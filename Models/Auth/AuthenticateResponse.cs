namespace apidemo.Models.Auth;
using apidemo.Entities;

public class AuthenticateResponse
{
    
    public int Id { get; set; }
    public string Username { get; set; }
    public string Token { get; set; }


    public AuthenticateResponse(Users user, string token)
    {
        Id = user.Id;
        Username = user.Username;
        Token = token;
    }
}