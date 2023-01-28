using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Phoenix.Infrastructure.OpenApi;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class SwaggerHeaderAttribute : Attribute
{
    public string HeaderName { get; }
    public string? Description { get; }
    public string? DefaultValue { get; }
    public bool IsRequired { get; }

    public SwaggerHeaderAttribute(
        string headerName,
        string? description = null,
        string? defaultValue = null,
        bool isRequired = false)
    {
        HeaderName = headerName;
        Description = description;
        DefaultValue = defaultValue;
        IsRequired = isRequired;
    }

}
public class SwaggerHeaderParameter : IOperationFilter
{
    public void Apply(
        OpenApiOperation operation,
        OperationFilterContext context)
    {
        if (operation.Parameters == null)
            operation.Parameters = new List<OpenApiParameter>();

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "TenantId",
            In = ParameterLocation.Header,
            Required = false,
            Schema = new OpenApiSchema
            {
                Type = "string",
                Nullable = true,
                Default = null,
                MaxLength = 450
            }
        });
    }
}
