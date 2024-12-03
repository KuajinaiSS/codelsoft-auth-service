using System.ComponentModel.DataAnnotations;

namespace auth_service.DTOs.Token;

public class TokenRevokeRequest
{
    [Required]
    public string Token { get; set; } = null!;
    
}