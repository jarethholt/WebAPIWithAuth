using System.Text.Json.Serialization;

namespace WebAPIWithAuth.Models;

public class User
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public required string UserName { get; set; }

    [JsonIgnore]
    public required string Password { get; set; }
    public bool IsActive { get; set; }
}
