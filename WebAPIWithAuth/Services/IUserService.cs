using WebAPIWithAuth.Models;

namespace WebAPIWithAuth.Services;

public interface IUserService
{
    Task<AuthenticateResponse?> Authenticate(AuthenticateRequest request);
    Task<IEnumerable<User>> GetActiveUsers();
    Task<User?> GetUserById(int id);
    Task<User?> AddAndUpdateUser(User user);
}
