namespace WebAPIWithAuth.Models;

public class AuthenticateResponse(User user, string token)
{
    public int Id { get; set; } = user.Id;
    public string FirstName { get; set; } = user.FirstName;
    public string LastName { get; set; } = user.LastName;
    public string UserName { get; set; } = user.UserName;
    public string Token { get; set; } = token;
}
