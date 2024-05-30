using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPIWithAuth.AuthHelpers;

[AttributeUsage(
    AttributeTargets.Class | AttributeTargets.Method,
    AllowMultiple = true,
    Inherited = true)]
public class ApiKeyAttribute(IConfiguration config) : Attribute, IAuthorizationFilter
{
    private const string _ApiKeyName = "XApiKey";
    private readonly IConfiguration _config = config;

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var httpContext = context.HttpContext;
        bool apiKeyPresentInHeader = httpContext.Request.Headers
            .TryGetValue(_ApiKeyName, out var extractedApiKey);
        var apiKey = _config[_ApiKeyName];

        if ((apiKeyPresentInHeader && apiKey == extractedApiKey)
            || httpContext.Request.Path.StartsWithSegments("/swagger"))
            return;
        else
            context.Result = new UnauthorizedResult();
    }
}
