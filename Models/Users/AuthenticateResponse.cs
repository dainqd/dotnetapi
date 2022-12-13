using apidemo.Entities;

namespace apidemo.Models.Users;

public class AuthenticateResponse
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Birthday { get; set; }
    public string Gender { get; set; } 
    public string Address { get; set; } 
    public Role Role { get; set; }
    public string Token { get; set; }
}