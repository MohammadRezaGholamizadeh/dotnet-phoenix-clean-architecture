using Hangfire.Client;
using Hangfire.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Phoenix.Infrastructure.Common;
using Phoenix.SharedConfiguration.Authorization;

namespace Phoenix.Infrastructure.BackgroundJobs;

public class PhoenixJobFilter : IClientFilter
{
    private static ILog Logger;
    private readonly IServiceProvider _services;
    public PhoenixJobFilter(IServiceProvider services)
    {
        _services = services;
        Logger = LogProvider.GetCurrentClassLogger();
    }


    public void OnCreating(
        CreatingContext context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        Logger.InfoFormat(
            $"Set UserId parameters to job " +
            $"{context.Job.Method.ReflectedType?.FullName}" +
            $".{context.Job.Method.Name}...");

        using var serviceScope = _services.CreateScope();

        var httpContext =
            serviceScope.ServiceProvider
            .GetRequiredService<IHttpContextAccessor>()?.HttpContext;

        GaurdAgainstHttpContextNotExist(httpContext);

        string? userId = httpContext!.User.GetUserId();
        context.SetJobParameter(QueryStringKeys.UserId, userId);
    }

    public void OnCreated(CreatedContext context)
    {
        var parameters =
            context.Parameters
                   .Select(_ => $"{_.Key} = {_.Value}")
                   .Aggregate((p, p2) => $"{p} ; {p2} {Environment.NewLine}");
        Logger.InfoFormat(
          $"Job created Successfully with parameters  : {parameters}");
    }


    private static void GaurdAgainstHttpContextNotExist(HttpContext? httpContext)
    {
        _ = httpContext
             ?? throw new InvalidOperationException(
                 "Can't create Job without HttpContext.");
    }
}
