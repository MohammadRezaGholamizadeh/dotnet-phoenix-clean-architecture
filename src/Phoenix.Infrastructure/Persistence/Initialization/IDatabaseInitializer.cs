namespace Phoenix.Infrastructure.Persistence.Initialization;

internal interface IDatabaseInitializer
{
    void InitializeDatabasesAsync();
}
