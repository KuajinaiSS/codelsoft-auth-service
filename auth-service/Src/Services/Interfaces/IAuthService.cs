using auth_service.DTOs.Auth;

namespace auth_service.Services.Interfaces;

public interface IAuthService
{
    /// <summary>
    /// Authenticates a user by attempting to log in using the provided credentials.
    /// </summary>
    /// <param name="loginRequestDto">A data transfer object containing login credentials.</param>
    /// <returns>
    ///  A task that represents the asynchronous login operation and returns a <see cref="LoginResponseDto"/>.
    /// </returns>
    public Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);

    /// <summary>
    /// Gets the email inside the JWT token identified as <see cref="ClaimTypes.Email"/> 
    /// </summary>
    /// <returns>email</returns>
    /// <exception cref="UnauthorizedAccessException">
    /// Thrown when the token do not contain <see cref="ClaimTypes.Email"/>
    /// </exception>
    public string GetUserEmailInToken();

    /// <summary>
    /// Gets the rolename inside the JWT token identified as <see cref="ClaimTypes.Role"/> 
    /// </summary>
    /// <returns>role</returns>
    /// <exception cref="UnauthorizedAccessException">
    /// Thrown when the token do not contain <see cref="ClaimTypes.Role"/>
    /// </exception>
    public string GetUserRoleInToken();
    
}