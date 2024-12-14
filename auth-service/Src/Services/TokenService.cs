using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using auth_service.DTOs.Token;
using auth_service.Exceptions;
using auth_service.Models;
using auth_service.Repositories.Interfaces;
using auth_service.Services.Interfaces;
using DotNetEnv;

namespace auth_service.Services;

public class TokenService : ITokenService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    private readonly IMapperService _mapperService;
    private readonly IHttpContextAccessor _ctxAccesor;
    private readonly string _jwtSecret;

    public TokenService(IUnitOfWork unitOfWork,
        IConfiguration configuration,
        IMapperService mapperService,
        IHttpContextAccessor ctxAccesor)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
        _mapperService = mapperService;
        _ctxAccesor = ctxAccesor;
        _jwtSecret = Env.GetString("JWT_SECRET") ?? throw new InvalidJwtException("JWT_SECRET not found");
    }


    public async Task<TokenRevokeResponse> TokenRevoke(TokenRevokeRequest tokenRevokeRequest)
    {
        var token = _mapperService.Map<TokenRevokeRequest, TokenBlackList>(tokenRevokeRequest);
        await _unitOfWork.TokensRepository.Insert(token);
        
        var response = _mapperService.Map<TokenRevokeRequest, TokenRevokeResponse>(tokenRevokeRequest);
        return response;
    }

    public async Task<TokenValidateResponse> TokenValidate(TokenValidateRequest tokenValidateRequest)
    {
        var isExpiredToken = IsExpiredToken(tokenValidateRequest.Token);
        var isInBlackList = await _unitOfWork.TokensRepository.GetTokenBlackList(tokenValidateRequest.Token);

        Console.Write("blackList: "+ isInBlackList + "\nExpired: " + isExpiredToken);
        var response = new TokenValidateResponse
        {
            IsValid = isExpiredToken && isInBlackList == null
        };
        
        return response;
    }

    private static bool IsExpiredToken(string tokenRequest)
    {
        var jwtHandler = new JwtSecurityTokenHandler();
        var token = jwtHandler.ReadJwtToken(tokenRequest);

        var expiresAt = token.Claims.FirstOrDefault(c => c.Type == "exp")?.Value;

        if(expiresAt == null) throw new InvalidJwtException("Invalid token: expiration claim missing");
        
        var expiresAtSeconds = long.Parse(expiresAt);
        var currentTimeSeconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        
        // retorna true si es que el tiempo de expiracion supera al actual
        return expiresAtSeconds > currentTimeSeconds;
    }
}