using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebAPIWithAuth.Data;
using WebAPIWithAuth.Models;

namespace WebAPIWithAuth.Services;

public class UserService(IOptions<AppSettings> settings, HeroContext context) : IUserService
{
    private readonly AppSettings _settings = settings.Value;
    private readonly HeroContext _context = context;

    public async Task<User?> AddAndUpdateUser(User user)
    {
        bool success = false;
        if (user.Id > 0)
        {
            var currentUser = await _context.User.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (currentUser is not null)
            {
                currentUser.FirstName = user.FirstName;
                currentUser.LastName = user.LastName;
                _context.User.Update(currentUser);
                success = await _context.SaveChangesAsync() > 0;
            }
        }
        else
        {
            await _context.User.AddAsync(user);
            success = await _context.SaveChangesAsync() > 0;
        }

        return success ? user : null;
    }

    public async Task<AuthenticateResponse?> Authenticate(AuthenticateRequest request)
    {
        var user = await _context.User.SingleOrDefaultAsync(
            u => u.UserName == request.UserName && u.Password == request.Password);
        if (user is null)
            return null;

        var token = await GenerateJwtTokenAsync(user);
        return new AuthenticateResponse(user, token);
    }

    public async Task<User?> GetUserById(int id) =>
        await _context.User.FirstOrDefaultAsync(u => u.Id == id);

    public async Task<IEnumerable<User>> GetActiveUsers() =>
        await _context.User.Where(u => u.IsActive).ToListAsync();

    private async Task<string> GenerateJwtTokenAsync(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = await Task.Run(() =>
        {
            var key = Encoding.ASCII.GetBytes(_settings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity([new Claim("id", user.Id.ToString())]),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            return tokenHandler.CreateToken(tokenDescriptor);
        });

        return tokenHandler.WriteToken(token);
    }
}
