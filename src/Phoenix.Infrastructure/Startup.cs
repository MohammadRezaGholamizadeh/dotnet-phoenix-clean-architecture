using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Phoenix.Infrastructure.ApiVersionings;
using Phoenix.Infrastructure.Auth;
using Phoenix.Infrastructure.BackgroundJobs;
using Phoenix.Infrastructure.Caching;
using Phoenix.Infrastructure.Common;
using Phoenix.Infrastructure.Cors;
using Phoenix.Infrastructure.FileStorage;
using Phoenix.Infrastructure.Localization;
using Phoenix.Infrastructure.Mailing;
using Phoenix.Infrastructure.Mapping;
using Phoenix.Infrastructure.Middleware;
using Phoenix.Infrastructure.Multitenancy;
using Phoenix.Infrastructure.Notifications;
using Phoenix.Infrastructure.OpenApi;
using Phoenix.Infrastructure.Persistence;
using Phoenix.Infrastructure.Persistence.Initialization;
using Phoenix.Infrastructure.SecurityHeaders;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

[assembly: InternalsVisibleTo("Infrastructure.Test")]

namespace Phoenix.Infrastructure;

public static class Startup
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration config)
    {
        MapsterSettings.Configure();
        services.AddHealthChecks();
        services.AddMvcCore()
            .AddAuthorization(options =>
             options.AddPolicy("Administrator", policy =>
             policy.RequireAuthenticatedUser()
            .RequireRole("Administrator")))
            .AddJsonOptions(jsonOptions =>
            {
                jsonOptions.JsonSerializerOptions
                           .DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                jsonOptions.JsonSerializerOptions
                           .PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                jsonOptions.JsonSerializerOptions
                           .DefaultIgnoreCondition =
                                JsonIgnoreCondition.WhenWritingNull;
                jsonOptions.JsonSerializerOptions
                           .Converters.Add(new EnumConverter());
                jsonOptions.JsonSerializerOptions
                           .Converters.Add(new UtcDateTimeConverter());
                jsonOptions.JsonSerializerOptions
                           .Converters.Add(new GuidNormalizeConverter());
                jsonOptions.JsonSerializerOptions
                           .Converters.Add(new TimeSpanConverter());
            });


        return services
            .AddHttpContextAccessor()
            .AddApiVersioning(config)
            .AddAuth(config)
            .AddBackgroundJobs(config)
            .AddCaching(config)
            .AddCorsPolicy(config)
            .AddExceptionMiddleware()
            .AddPOLocalization(config)
            .AddMailing(config)
            .AddEFDataContext(config)
            .AddNotifications(config)
            .AddOpenApiDocumentation(config)
            .AddPersistence(config)
            .AddRequestLogging(config)
            .AddRouting(options => options.LowercaseUrls = true)
            .AddServices();

    }

    public static void InitializeDatabasesAsync(
        this IServiceProvider services)
    {
        using (var scope = services.CreateScope())
        {
            scope.ServiceProvider
                  .GetRequiredService<IDatabaseInitializer>()
                  .InitializeDatabasesAsync();
        }
    }

    public static IApplicationBuilder UseInfrastructure(
        this IApplicationBuilder builder,
        IConfiguration config)
    {
        new ExceptionHandelinConfig()
            .ConfigureExceptionHandeling(builder);

        builder
            .UseRequestLocalization()
            .UseStaticFiles()
            .UseSecurityHeaders(config)
            .UseFileStorage()
            .UseExceptionMiddleware()
            .UseRouting()
            .UseAuthentication()
            .UseAuthorization()
            .UseEndpoints(endpoints =>
               {
                   endpoints.MapEndpoints();
               })
            .UseCorsPolicy()
            .UseCurrentUser()
            .UseRequestLogging(config)
            .UseHangfireDashboard(config)
            .UseOpenApiDocumentation(config);
        return builder;
    }


    public static IEndpointRouteBuilder MapEndpoints(
        this IEndpointRouteBuilder builder)
    {
        builder.MapControllers();
        builder.MapHealthCheck();
        builder.MapNotifications();
        return builder;
    }

    private static IEndpointConventionBuilder MapHealthCheck(
        this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapHealthChecks("/api/health")
                        .RequireAuthorization();

    }
    class EnumConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsEnum;
        }

        public override JsonConverter CreateConverter(
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            var converterType =
                typeof(EnumConverter<>)
                .MakeGenericType(typeToConvert);
            return Activator.CreateInstance(converterType)
                   as JsonConverter;
        }
    }

    class GuidNormalizeConverter : JsonConverter<Guid>
    {
        public override Guid Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            return new Guid(reader.GetString());
        }

        public override void Write(
            Utf8JsonWriter writer,
            Guid value,
            JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("N"));
        }
    }

    class UtcDateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            return reader.GetDateTime().ToUniversalTime();
        }

        public override void Write(
            Utf8JsonWriter writer,
            DateTime value,
            JsonSerializerOptions options)
        {
            var zonedDateValue = GetZonedDate(value);
            writer.WriteStringValue(zonedDateValue.ToUniversalTime());
        }

        private DateTime GetZonedDate(DateTime value)
        {
            return value.Kind ==
                DateTimeKind.Unspecified
                ? DateTime.SpecifyKind(value, DateTimeKind.Utc)
                : value;
        }
    }

    class TimeSpanConverter : JsonConverter<TimeSpan>
    {
        public override TimeSpan Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            return TimeSpan.Parse(reader.GetString());
        }

        public override void Write(
            Utf8JsonWriter writer,
            TimeSpan value,
            JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("hh\\:mm\\:ss"));
        }
    }

    class EnumConverter<TEnum> : JsonConverter<TEnum>
        where TEnum : struct, Enum
    {
        private readonly Type _enumType;
        private readonly TypeCode _enumTypeCode;

        public EnumConverter()
        {
            _enumType = typeof(TEnum);
            _enumTypeCode = Type.GetTypeCode(typeof(TEnum));
        }

        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsEnum;
        }

        public override TEnum Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            return _enumTypeCode switch
            {
                TypeCode.Byte => Read(reader.GetByte()),
                TypeCode.SByte => Read(reader.GetSByte()),
                TypeCode.Int16 => Read(reader.GetInt16()),
                TypeCode.UInt16 => Read(reader.GetUInt16()),
                TypeCode.Int32 => Read(reader.GetInt32()),
                TypeCode.UInt32 => Read(reader.GetUInt32()),
                TypeCode.Int64 => Read(reader.GetInt64()),
                TypeCode.UInt64 => Read(reader.GetUInt64()),
                _ => throw new InvalidOperationException()
            };
        }

        private void CheckValueIsValid<T>(T value)
        {
            if (!Enum.IsDefined(_enumType, value))
                throw new InvalidOperationException(
                    $"Invalid enumeration '{_enumType.Name}' value '{value}'");
        }

        public override void Write(
            Utf8JsonWriter writer,
            TEnum value,
            JsonSerializerOptions options)
        {
            switch (_enumTypeCode)
            {
                case TypeCode.Byte:
                    writer.WriteNumberValue(As<byte>(value));
                    break;
                case TypeCode.SByte:
                    writer.WriteNumberValue(As<sbyte>(value));
                    break;
                case TypeCode.Int16:
                    writer.WriteNumberValue(As<short>(value));
                    break;
                case TypeCode.UInt16:
                    writer.WriteNumberValue(As<ushort>(value));
                    break;
                case TypeCode.Int32:
                    writer.WriteNumberValue(As<int>(value));
                    break;
                case TypeCode.UInt32:
                    writer.WriteNumberValue(As<uint>(value));
                    break;
                case TypeCode.Int64:
                    writer.WriteNumberValue(As<long>(value));
                    break;
                case TypeCode.UInt64:
                    writer.WriteNumberValue(As<ulong>(value));
                    break;
                default: throw new InvalidOperationException();
            }
        }

        private TEnum Read<T>(T value)
        {
            CheckValueIsValid(value);
            return Enum.Parse<TEnum>(value.ToString());
        }

        private _enum As<_enum>(object value)
        {
            CheckValueIsValid(value);
            return (_enum)value;
        }
    }
}
