using Microsoft.Extensions.DependencyInjection;

namespace Phoenix.Infrastructure.Persistence.Initialization;

internal class CustomSeederRunner
{
    private readonly ICustomSeeder[] _customSeeders;

    public CustomSeederRunner(IServiceProvider serviceProvider)
    {
        _customSeeders = 
            serviceProvider.GetServices<ICustomSeeder>().ToArray();
    }

    public async Task RunCustomSeederAsync(
        CancellationToken cancellationToken)
    {
        foreach (var seeder in _customSeeders)
        {
            await seeder.InitializeAsync(cancellationToken);
        }
    }
}
