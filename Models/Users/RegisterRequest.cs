using System.ComponentModel.DataAnnotations;

namespace apidemo.Models.Users;

public class RegisterRequest
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }
}