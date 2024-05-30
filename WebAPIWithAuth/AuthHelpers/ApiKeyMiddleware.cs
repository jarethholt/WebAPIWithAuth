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
