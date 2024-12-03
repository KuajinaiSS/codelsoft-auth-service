using auth_service.DTOs.Token;
using Microsoft.AspNetCore.Mvc;

namespace auth_service.Services.Interfaces;

public interface ITokenService
{
    public Task<TokenRevokeResponse> TokenRevoke(TokenRevokeRequest tokenRevokeRequest);
    
    public Task<TokenValidateResponse> TokenValidate(TokenValidateRequest tokenValidateRequest);
}