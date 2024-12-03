using auth_service.DTOs.Models;

namespace auth_service.DTOs.Auth;

public class LoginResponseDto : BaseModelDto
{
    public string Email { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string Token { get; set; } = null!;
}