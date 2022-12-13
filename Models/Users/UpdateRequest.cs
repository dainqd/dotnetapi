using System.ComponentModel.DataAnnotations;
using apidemo.Entities;

namespace apidemo.Models.Users;

public class UpdateRequest
{
    public string FirstName { get; set; } = "";
    public string LastNaqme { get; set; } = "";
    [Required]
    public string Username { get; set; }
    public string Email { get; set; } = "";
    public string PhoneNumber { get; set; } = "";

    [Required]
    [EnumDataType(typeof(Role))]
    public string Role { get; set; } = "User";
}