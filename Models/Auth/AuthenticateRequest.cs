using System.ComponentModel.DataAnnotations;
using apidemo.Entities;

namespace apidemo.Models.Auth;

public class AuthenticateRequest
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
}