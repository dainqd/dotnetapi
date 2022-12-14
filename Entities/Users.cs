using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace apidemo.Entities;

public class Users
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string FirstName { get; set; } = "";
    public string LastName { get; set; }  = "";
    public string Username { get; set; }
    public string Email { get; set; } = "";
    public string PhoneNumber { get; set; }  = "";
    public string Birthday { get; set; } = "";
    public string Gender { get; set; } = "";
    public string Address { get; set; }  = "";
    
    [JsonIgnore]
    public string Password { get; set; }

    public Role Role { get; set; } = Role.User;
}
