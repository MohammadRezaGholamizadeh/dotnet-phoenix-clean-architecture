using System.Data;
using Dapper;
using Phoenix.Application.Common.Persistence;
using Phoenix.DataSources.Infrastructures.DBContexts;
using Phoenix.SharedConfiguration.Common.Contracts;

namespace Phoenix.Infrastructure.Persistence.Repositories
{
    public class DapperRepository : IDapperRepository
    {
        private readonly EFDataContext _dbContext;

        public DapperRepository(EFDataContext dbContext) => _dbContext = dbContext;

        public async Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
        where T : class, IEntity =>
            (await _dbContext.Connection.QueryAsync<T>(sql, param, transaction))
                .AsList();

        public async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
        where T : class, IEntity
        {
            //if (_dbContext.Model.GetMultiTenantEntityTypes().Any(t => t.ClrType == typeof(T)))
            //{
            //    sql = sql.Replace("@tenant", _dbContext.Set<TenantInfo>().Id);
            //}

            return await _dbContext.Connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction);
        }

        public Task<T> QuerySingleAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
        where T : class, IEntity
        {
            //if (_dbContext.Model.GetMultiTenantEntityTypes().Any(t => t.ClrType == typeof(T)))
            //{
            //    sql = sql.Replace("@tenant", _dbContext.Set<TenantInfo>().Id);
            //}

            return _dbContext.Connection.QuerySingleAsync<T>(sql, param, transaction);
        }
    }
}