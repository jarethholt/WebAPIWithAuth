namespace WebAPIWithAuth.Models;

public class Hero
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}
