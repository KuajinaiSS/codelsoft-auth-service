using System.ComponentModel.DataAnnotations;

namespace auth_service.Models;

public class Role : BaseModel
{
    [StringLength(250)]
    public string Name { get; set; } = null!;
    
}