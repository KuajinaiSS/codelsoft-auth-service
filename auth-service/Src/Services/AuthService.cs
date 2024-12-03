using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using auth_service.Common.Constants;
using auth_service.DTOs.Auth;
using auth_service.Exceptions;
using auth_service.Models;
using auth_service.Repositories.Interfaces;
using auth_service.Services.Interfaces;
using DotNetEnv;
using Microsoft.IdentityModel.Tokens;

namespace auth_service.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IMapperService _mapperService;
        private readonly IHttpContextAccessor _ctxAccesor;
        private readonly string _jwtSecret;

        public AuthService(IUnitOfWork unitOfWork,
        IConfiguration configuration,
        IMapperService mapperService,
        IHttpContextAccessor ctxAccesor
        )
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _mapperService = mapperService;
            _ctxAccesor = ctxAccesor;
            _jwtSecret = Env.GetString("JWT_SECRET") ?? throw new InvalidJwtException("JWT_SECRET not found");
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            // obtener al usuario por base de datos
            var user = await _unitOfWork.UsersRepository.GetByEmail(loginRequestDto.Email)
                ?? throw new InvalidCredentialException("Invalid Credentials");

            // Verificar contraseña
            var verifyPassword = BCrypt.Net.BCrypt.Verify(loginRequestDto.Password, user.HashedPassword);
            if (!verifyPassword)
                throw new InvalidCredentialException("Invalid Credentials");

            // Verificar si es que esta habilitado
            if (!user.IsEnabled)
                throw new DisabledUserException("User is not enabled - Contact an administrator");

            // Generar token y asignarlo
            var token = CreateToken(user.Email, user.Role.Name);
            user.Token = token;
            await _unitOfWork.UsersRepository.Update(user);
            
            
            var response = _mapperService.Map<User, LoginResponseDto>(user);
            MapMissingFields(user, token, response);
            return response;
        }

        //TODO: Refactor this to MapperService
        private static void MapMissingFields(User createdUser, string token, LoginResponseDto response)
        {
            response.Token = token;
            response.Role = createdUser.Role.Name;
        }
        
        private string CreateToken(string email, string role)
        {
            var claims = new List<Claim>{
                new (ClaimTypes.Email, email),
                new (ClaimTypes.Role, role),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(2),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        public string GetUserEmailInToken()
        {
            var httpUser = GetHttpUser();

            //Get Claims from JWT
            var userEmail = httpUser.FindFirstValue(ClaimTypes.Email) ??
                throw new UnauthorizedAccessException("Invalid user email in token");
            return userEmail;
        }

        public string GetUserRoleInToken()
        {
            var httpUser = GetHttpUser();

            //Get Claims from JWT
            var userRole = httpUser.FindFirstValue(ClaimTypes.Role) ??
                throw new UnauthorizedAccessException("Invalid role in token");
            return userRole;
        }

        private ClaimsPrincipal GetHttpUser()
        {
            //Check if the HttpContext is available to work with
            return (_ctxAccesor.HttpContext?.User) ??
                throw new UnauthorizedAccessException();
        }
    
}