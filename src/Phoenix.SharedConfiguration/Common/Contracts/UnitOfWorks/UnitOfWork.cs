namespace Phoenix.SharedConfiguration.Common.Contracts.UnitOfWorks
{
    public interface UnitOfWork
    {
        Task SaveAllChangesAsync();
        void SaveAllChanges();
        Task BeginTransaction();
        Task Commit();
        Task CommitPartial();
    }
}
