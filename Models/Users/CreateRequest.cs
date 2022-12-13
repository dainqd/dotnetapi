using System.ComponentModel.DataAnnotations;
using apidemo.Entities;

namespace apidemo.Models.Users;

public class CreateRequest
{
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    [Required]
    public string Username { get; set; }
    public string Email { get; set; } = "";
    public string PhoneNumber { get; set; } = "";
    
    public string Birthday { get; set; }
    public string Gender { get; set; } = "";
    public string Address { get; set; }  = "";

    [Required]
    [EnumDataType(typeof(Role))]
    public string Role { get; set; } = "User";
    
    [Required]
    [MinLength(6)]
    public string Password { get; set; }

    [Required]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }
}