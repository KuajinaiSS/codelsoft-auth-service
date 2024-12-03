using System.ComponentModel.DataAnnotations;

namespace auth_service.Models;

public class User : BaseModel
{

    [StringLength(250)]
    public string Email { get; set; } = null!;

    [StringLength(250)]
    public string HashedPassword { get; set; } = null!;
    
    public string? Token { get; set; } = null!;
    public bool IsEnabled { get; set; } = true;
    
    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;
    
}