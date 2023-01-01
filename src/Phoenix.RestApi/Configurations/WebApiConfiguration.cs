using FluentValidation;
using MediatR;
using System.Reflection;

namespace Phoenix.RestApi.Configurations
{
    public static class WebApiConfiguration
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            return services
                .AddValidatorsFromAssembly(assembly)
                .AddMediatR(assembly);
        }
    }
}
