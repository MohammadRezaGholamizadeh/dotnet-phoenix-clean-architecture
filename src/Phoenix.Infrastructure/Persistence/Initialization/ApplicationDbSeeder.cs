﻿//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using Phoenix.Infrastructure.Identity;
//using Phoenix.Infrastructure.Multitenancy;
//using Phoenix.Infrastructure.Persistence.Context;
//using Phoenix.SharedConfiguration.Authorization;
//using Phoenix.SharedConfiguration.Multitenancy;

//namespace Phoenix.Infrastructure.Persistence.Initialization;

//internal class ApplicationDbSeeder
//{
//    private readonly RoleManager<ApplicationRoles> _roleManager;
//    private readonly UserManager<ApplicationUser> _userManager;
//    private readonly CustomSeederRunner _seederRunner;
//    private readonly ILogger<ApplicationDbSeeder> _logger;

//    public ApplicationDbSeeder(RoleManager<ApplicationRoles> roleManager, UserManager<ApplicationUser> userManager, CustomSeederRunner seederRunner, ILogger<ApplicationDbSeeder> logger)
//    {
//        _roleManager = roleManager;
//        _userManager = userManager;
//        _seederRunner = seederRunner;
//        _logger = logger;
//    }

//    public async Task SeedDatabaseAsync(
//        ApplicationDbContext dbContext, CancellationToken cancellationToken)
//    {
//        await SeedRolesAsync(dbContext);
//        await SeedAdminUserAsync();
//        await _seederRunner.RunSeedersAsync(cancellationToken);
//    }

//    private async Task SeedRolesAsync(ApplicationDbContext dbContext)
//    {
//        foreach (string roleName in PhoenixRoles.DefaultRoles)
//        {
//            if (await _roleManager.Roles.SingleOrDefaultAsync(r => r.Name == roleName)
//                is not ApplicationRoles role)
//            {
//                // Create the role
//                _logger.LogInformation("Seeding {role} Role for '{tenantId}' Tenant.", roleName, _currentTenant.Id);
//                role = new ApplicationRoles(roleName, $"{roleName} Role for Tenant");
//                await _roleManager.CreateAsync(role);
//            }

//            // Assign permissions
//            if (roleName == PhoenixRoles.Basic)
//            {
//                await AssignPermissionsToRoleAsync(dbContext, PhoenixPermissions.Basic, role);
//            }
//            else if (roleName == PhoenixRoles.Admin)
//            {
//                await AssignPermissionsToRoleAsync(dbContext, PhoenixPermissions.Admin, role);

//                if (_currentTenant.Id == MultitenancyConstants.Root.Id)
//                {
//                    await AssignPermissionsToRoleAsync(dbContext, PhoenixPermissions.Root, role);
//                }
//            }
//        }
//    }

//    private async Task AssignPermissionsToRoleAsync(ApplicationDbContext dbContext, IReadOnlyList<PhoenixPermission> permissions, ApplicationRoles role)
//    {
//        var currentClaims = await _roleManager.GetClaimsAsync(role);
//        foreach (var permission in permissions)
//        {
//            if (!currentClaims.Any(c => c.Type == PhoenixClaims.Permission && c.Value == permission.Name))
//            {
//                _logger.LogInformation("Seeding {role} Permission '{permission}' for '{tenantId}' Tenant.", role.Name, permission.Name, _currentTenant.Id);
//                dbContext.RoleClaims.Add(new ApplicationRoleClaim
//                {
//                    RoleId = role.Id,
//                    ClaimType = PhoenixClaims.Permission,
//                    ClaimValue = permission.Name,
//                    CreatedBy = "ApplicationDbSeeder"
//                });
//                await dbContext.SaveChangesAsync();
//            }
//        }
//    }

//    private async Task SeedAdminUserAsync()
//    {
//        if (string.IsNullOrWhiteSpace(_currentTenant.Id) || string.IsNullOrWhiteSpace(_currentTenant.AdminEmail))
//        {
//            return;
//        }

//        if (await _userManager.Users.FirstOrDefaultAsync(u => u.Email == _currentTenant.AdminEmail)
//            is not ApplicationUser adminUser)
//        {
//            string adminUserName = $"{_currentTenant.Id.Trim()}.{PhoenixRoles.Admin}".ToLowerInvariant();
//            adminUser = new ApplicationUser
//            {
//                FirstName = _currentTenant.Id.Trim().ToLowerInvariant(),
//                LastName = PhoenixRoles.Admin,
//                Email = _currentTenant.AdminEmail,
//                UserName = adminUserName,
//                EmailConfirmed = true,
//                PhoneNumberConfirmed = true,
//                NormalizedEmail = _currentTenant.AdminEmail?.ToUpperInvariant(),
//                NormalizedUserName = adminUserName.ToUpperInvariant(),
//                IsActive = true
//            };

//            _logger.LogInformation("Seeding Default Admin User for '{tenantId}' Tenant.", _currentTenant.Id);
//            var password = new PasswordHasher<ApplicationUser>();
//            adminUser.PasswordHash = password.HashPassword(adminUser, MultitenancyConstants.DefaultPassword);
//            await _userManager.CreateAsync(adminUser);
//        }

//        // Assign role to user
//        if (!await _userManager.IsInRoleAsync(adminUser, PhoenixRoles.Admin))
//        {
//            _logger.LogInformation("Assigning Admin Role to Admin User for '{tenantId}' Tenant.", _currentTenant.Id);
//            await _userManager.AddToRoleAsync(adminUser, PhoenixRoles.Admin);
//        }
//    }
//}
