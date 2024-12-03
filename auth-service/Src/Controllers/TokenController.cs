using auth_service.DTOs.Token;
using auth_service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace auth_service.Controllers;

public class TokenController : BaseApiController
{
    private readonly ITokenService _tokenService;

    public TokenController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpPost("revoke")]
    public async Task<ActionResult<TokenRevokeResponse>> RevokeToken([FromBody] TokenRevokeRequest tokenRevokeRequest)
    {
        var response = await _tokenService.TokenRevoke(tokenRevokeRequest);
        return Ok(response);
    }

    [HttpPost("validate")]
    public async Task<ActionResult<TokenValidateResponse>> ValidateToken([FromBody] TokenValidateRequest tokenValidateRequest)
    {
        var response = await _tokenService.TokenValidate(tokenValidateRequest);
        return Ok(response);
    }
}