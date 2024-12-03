using System.ComponentModel.DataAnnotations;

namespace auth_service.Models;

public class TokenBlackList
{
    public int Id { get; set; }
    
    public string Token { get; set; }
}