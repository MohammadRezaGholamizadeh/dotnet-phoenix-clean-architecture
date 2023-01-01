using Hangfire;
using Hangfire.Server;
using Microsoft.Extensions.DependencyInjection;
using Phoenix.Infrastructure.Auth;
using Phoenix.Infrastructure.Common;

namespace Phoenix.Infrastructure.BackgroundJobs;

public class PhoenixJobActivator : JobActivator
{
    private readonly IServiceScopeFactory _serviceScopFactory;

    public PhoenixJobActivator(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopFactory = serviceScopeFactory
            ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
    }

    public override JobActivatorScope BeginScope(PerformContext context)
    {
        return new Scope(context, _serviceScopFactory.CreateScope());
    }

    private class Scope : JobActivatorScope, IServiceProvider
    {
        private readonly PerformContext _context;
        private readonly IServiceScope _scope;

        public Scope(PerformContext context, IServiceScope scope)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _scope = scope ?? throw new ArgumentNullException(nameof(scope));
            GetJobParameters();
        }

        private void GetJobParameters()
        {
            string userId =
                 _context.GetJobParameter<string>(QueryStringKeys.UserId);
            if (!string.IsNullOrEmpty(userId))
            {
                _scope.ServiceProvider.GetRequiredService<ICurrentUserInitializer>()
                    .SetCurrentUserId(userId);
            }
        }

        public override object Resolve(Type type)
        {
            return ActivatorUtilities.GetServiceOrCreateInstance(this, type);
        }

        object? IServiceProvider.GetService(Type serviceType)
        {
            if (serviceType.Equals(typeof(PerformContext)))
                return _context;
            return _scope.ServiceProvider.GetService(serviceType);
        }

    }
}
