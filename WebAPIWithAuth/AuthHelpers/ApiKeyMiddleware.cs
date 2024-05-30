using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebAPIWithAuth.AuthHelpers;

public class ApiKeyMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;
    private const string _ApiKeyName = "XApiKey";

    public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
    {
        bool apiKeyPresentInHeader = context.Request.Headers
            .TryGetValue(_ApiKeyName, out var extractedApiKey);
        var apiKey = configuration[_ApiKeyName];

        // Check that keys match OR whether we're in Swagger debug
        if ((apiKeyPresentInHeader && apiKey == extractedApiKey)
            || context.Request.Path.StartsWithSegments("/swagger"))
        {
            await _next(context);
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Invalid API key");
        }
    }
}

public static class ApiKeyMiddlewareExtension
{
    public static IApplicationBuilder UseApiKey(this IApplicationBuilder builder) =>
        builder.UseMiddleware<ApiKeyMiddleware>();
}

public static class SwaggerApiKeySecurity
{
    public static void AddSwaggerApiKeySecurity(this SwaggerGenOptions opts)
    {
        opts.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
        {
            Description = "Api key must appear in the header",
            Type = SecuritySchemeType.ApiKey,
            Name = "XApiKey",
            In = ParameterLocation.Header,
            Scheme = "ApiKeyScheme"
        });

        OpenApiSecurityScheme key = new()
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "ApiKey"
            },
            In = ParameterLocation.Header
        };

        opts.AddSecurityRequirement(new OpenApiSecurityRequirement
            { { key, new List<string>() } });
    }
}
