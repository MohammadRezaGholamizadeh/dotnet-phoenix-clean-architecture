using Phoenix.DataSources.Infrastructures.DBContexts;
using Phoenix.SharedConfiguration.Common.Contracts.UnitOfWorks;

namespace Phoenix.DataSources.Infrastructures.UnitOfWorks
{
    public class EFUnitOfWork : UnitOfWork
    {
        private readonly EFDataContext _context;

        public EFUnitOfWork(EFDataContext context)
        {
            _context = context;
        }
        public async Task BeginTransaction()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
            await _context.Database.CommitTransactionAsync();
        }

        public async Task CommitPartial()
        {
            await _context.SaveChangesAsync();
        }

        public void SaveAllChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveAllChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
