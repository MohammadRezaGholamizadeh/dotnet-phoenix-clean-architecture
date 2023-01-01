using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Phoenix.Infrastructure.SecurityHeaders;

internal static class SecurityHeaderSetting
{
    internal static IApplicationBuilder UseSecurityHeaders(
        this IApplicationBuilder app,
        IConfiguration config)
    {
        var configuration =
            config.GetSection(nameof(SecurityHeaderConfiguration))
            .Get<SecurityHeaderConfiguration>();

        if (configuration?.Enable is true)
        {
            app.Use(async (context, next) =>
            {
                if (!context.Response.HasStarted)
                {
                    if (!string.IsNullOrWhiteSpace(configuration.X_FrameOptions))
                    {
                        context.Response
                               .Headers.Add(HeaderNames.X_FRAMEOPTIONS, 
                                            configuration.X_FrameOptions);
                    }

                    if (!string.IsNullOrWhiteSpace(
                        configuration.X_Content_Type_Options))
                    {
                        context.Response
                               .Headers.Add(HeaderNames.X_CONTENT_TYPE_OPTIONS, 
                                            configuration.X_Content_Type_Options);
                    }

                    if (!string.IsNullOrWhiteSpace(configuration.Referrer_Policy))
                    {
                        context.Response
                               .Headers.Add(HeaderNames.REFERRER_POLICY, 
                                            configuration.Referrer_Policy);
                    }

                    if (!string.IsNullOrWhiteSpace(configuration.Permissions_Policy))
                    {
                        context.Response
                               .Headers.Add(HeaderNames.PERMISSIONS_POLICY, 
                                            configuration.Permissions_Policy);
                    }

                    if (!string.IsNullOrWhiteSpace(configuration.SameSite))
                    {
                        context.Response
                               .Headers.Add(HeaderNames.SAMESITE, 
                                            configuration.SameSite);
                    }

                    if (!string.IsNullOrWhiteSpace(configuration.X_XSS_Protection))
                    {
                        context.Response
                               .Headers.Add(HeaderNames.X_XSS_PROTECTION, 
                                            configuration.X_XSS_Protection);
                    }
                }
                await next();
            });
        }
        return app;
    }
}
