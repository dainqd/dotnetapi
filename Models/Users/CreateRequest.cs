using System.ComponentModel.DataAnnotations;
using apidemo.Entities;

namespace apidemo.Models.Users;

public class CreateRequest
{
    [Required]
    public string Username { get; set; }
    
    [Required]
    [EnumDataType(typeof(Role))]
    public string Role { get; set; }
    
    [Required]
    [MinLength(6)]
    public string Password { get; set; }

    [Required]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }
}