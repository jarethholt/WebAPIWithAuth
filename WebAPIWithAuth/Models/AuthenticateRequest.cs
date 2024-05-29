using System.ComponentModel;

namespace WebAPIWithAuth.Models;

public class AuthenticateRequest
{
    [DefaultValue("System")]
    public required string UserName { get; set; }
    [DefaultValue("System")]
    public required string Password { get; set; }
}
