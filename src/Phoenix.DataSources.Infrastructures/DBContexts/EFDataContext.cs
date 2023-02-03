using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Phoenix.Application.Common.Tokens;
using Phoenix.DataSources.Infrastructures.EntityMaps.EF.MigrationsDomain;
using Phoenix.Domain.Entities.ApplicationUsers;
using Phoenix.SharedConfiguration.Multitenancy;
using System.Data;
using System.Linq.Expressions;

namespace Phoenix.DataSources.Infrastructures.DBContexts
{
    public class EFDataContext : IdentityDataContext
    {
        private readonly string _tenantId;
        public EFDataContext(
            DbContextOptions<EFDataContext> options,
            UserTokenService userTokenService) 
            : base(options, userTokenService)
        {
            _tenantId = userTokenService.TenantId;
        }

        public EFDataContext(
            string connectionString,
            UserTokenService userTokenService) : this(
                new DbContextOptionsBuilder<EFDataContext>()
                .UseSqlServer(connectionString).Options,
                userTokenService)
        {
        }

        public IDbConnection Connection => Database.GetDbConnection();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .ApplyConfigurationsFromAssembly(
                 typeof(EFMigrationVersionInfoEntityMap).Assembly);

            Expression<Func<ITenant, bool>> filterExpr
               = ex => ex.TenantId == _tenantId;
            var entities =
                typeof(ApplicationUser)
                  .Assembly.GetTypes()
                  .Where(_ => _.IsAssignableTo(typeof(ITenant))
                           && !_.IsInterface);
            foreach (var item in entities.AsParallel())
            {
                var parameter = Expression.Variable(item);
                var body = ReplacingExpressionVisitor
                           .Replace(
                               filterExpr.Parameters.First(),
                               parameter,
                               filterExpr.Body);
                var lambdaExpression = Expression.Lambda(body, parameter);
                modelBuilder.Entity(item).Metadata
                       .SetQueryFilter(lambdaExpression);
            }
        }
    }
}