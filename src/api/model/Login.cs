using System.ComponentModel.DataAnnotations;

namespace api.model;
public class Login
{
    [Key]
    public long Id { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}