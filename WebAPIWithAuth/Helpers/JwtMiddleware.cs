using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebAPIWithAuth.Models;
using WebAPIWithAuth.Services;

namespace WebAPIWithAuth.Helpers;

public class JwtMiddleware(RequestDelegate next, IOptions<AppSettings> settings)
{
    private readonly RequestDelegate _next = next;
    private readonly AppSettings _settings = settings.Value;

    public async Task Invoke(HttpContext context, IUserService userService)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(' ').Last();
        if (token is not null)
            await AttachUserToContext(context, userService, token);
        await _next(context);
    }

    private async Task AttachUserToContext(HttpContext context, IUserService userService, string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_settings.Secret);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = int.Parse(jwtToken.Claims.First(c => c.Type == "id").Value);

            context.Items["User"] = await userService.GetUserById(userId);
        }
        catch { }
    }
}
