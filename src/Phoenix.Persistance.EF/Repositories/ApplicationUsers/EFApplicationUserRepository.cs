using Microsoft.EntityFrameworkCore;
using Phoenix.Application.Services.ApplicationUsers.Contracts;
using Phoenix.DataSources.Infrastructures.DBContexts;
using Phoenix.Domain.Entities.ApplicationUsers;

namespace Phoenix.Persistance.EF.Repositories.ApplicationUsers
{
    public class EFApplicationUserRepository
        : ApplicationUserRepository
    {
        private readonly DbSet<ApplicationUser> _applicationUsers;
        private readonly DbSet<ApplicationRole> _applicationRoles;
        private readonly DbSet<ApplicationUserRole> _applicationUserRoles;

        public EFApplicationUserRepository(EFDataContext context)
        {
            _applicationUsers = context.Set<ApplicationUser>();
            _applicationRoles = context.Set<ApplicationRole>();
            _applicationUserRoles = context.Set<ApplicationUserRole>();
        }
        public async Task<ApplicationUser?> FindUserByNationalCode(string nationalCode)
        {
            return
                await _applicationUsers
                      .SingleOrDefaultAsync(
                            _ => _.NationalCode == nationalCode);
        }
        public async Task<string?> GetUserIdByNationalCode(string nationalCode)
        {
            var applicationUser =
                await _applicationUsers
                      .SingleOrDefaultAsync(
                            _ => _.NationalCode == nationalCode);
            return applicationUser?.Id;
        }
        public async Task<bool> IsDuplicateNationalCode(
            string userId,
            string nationalCode)
        {
            return await
                _applicationUsers
                .AnyAsync(
                    _ => _.Id != userId
                      && _.NationalCode == nationalCode);
        }
        public async Task<GetApplicationUserDto?> GetUserById(string userId)
        {
            return await
                _applicationUsers
                .Where(_ => _.Id == userId)
                                          .Select(_ => new GetApplicationUserDto
                                          {
                                              NationalCode = _.NationalCode,
                                              FirstName = _.FirstName,
                                              LastName = _.LastName,
                                              MobileNumber = _.Mobile.MobileNumber,
                                              CountryCallingCode = _.Mobile.CountryCallingCode,
                                              IsActive = _.IsActive,
                                              IsAdmin = _applicationUserRoles
                                                        .FirstOrDefault(_ => _.UserId == userId) != null ?
                                                        _applicationUserRoles
                                                        .FirstOrDefault(_ => _.UserId == userId)!.RoleId
                                                        == GetRoleId(SystemRoles.Admin) : false
                                          })
                                         .SingleOrDefaultAsync();
        }
        public async Task<ApplicationUser?> FindUserById(string userId)
        {
            return await _applicationUsers.FindAsync(userId);
        }
        public async Task<IList<ApplicationUser>> GetAllRegistredUsers(
            string nationalCode,
            string countryCallingCode,
            string mobileNumber)
        {
            return await
                _applicationUsers
                .Where(_ => _.NationalCode == nationalCode ||
                           (_.Mobile.MobileNumber != null
                            && _.Mobile.MobileNumber == mobileNumber))
                .Select(_ => new ApplicationUser
                {
                    NationalCode = _.NationalCode,
                    FirstName = _.FirstName,
                    LastName = _.LastName,
                    Mobile = new Mobile()
                    {
                        MobileNumber = _.Mobile.MobileNumber,
                        CountryCallingCode = _.Mobile.CountryCallingCode
                    }
                })
                .ToListAsync();
        }
        public async Task<List<GetApplicationUserDto>> GetAllUsers()
        {
            return await (from user in _applicationUsers
                          select new GetApplicationUserDto()
                          {
                              Id = user.Id,
                              FirstName = user.FirstName,
                              LastName = user.LastName,
                              NationalCode = user.NationalCode,
                              MobileNumber = user.Mobile.MobileNumber,
                              CountryCallingCode = user.Mobile.CountryCallingCode,
                              IsActive = user.IsActive,
                              IsAdmin = _applicationUserRoles
                                        .FirstOrDefault(_ => _.UserId == user.Id) != null ?
                                        _applicationUserRoles
                                        .FirstOrDefault(_ => _.UserId == user.Id)!.RoleId
                                        == GetRoleId(SystemRoles.Admin) : false
                          }).ToListAsync();

        }
        public Task<bool> isExist(string userId)
        {
            return _applicationUsers.AnyAsync(_ => _.Id == userId);
        }
        public string? GetRoleId(string roleName)
        {
            return _applicationRoles
                   .SingleOrDefault(_ => _.NormalizedName == roleName.ToUpper())?.Id;
        }

        public async Task<bool> IsExistByNationalCode(string nationalCode)
        {
            return await _applicationUsers.AnyAsync(_ => _.NationalCode == nationalCode);
        }
    }
}
