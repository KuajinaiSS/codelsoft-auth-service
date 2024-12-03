namespace auth_service.DTOs.Token;

public class TokenValidateResponse
{
    public string Token { get; set; } = null!;
    
    public bool IsValid { get; set; } = false;
}
