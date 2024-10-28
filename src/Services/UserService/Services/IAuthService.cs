using Microsoft.AspNetCore.Identity;
using UserService.Models;
using UserService.Models.Dtos;

namespace UserService.Services;

public interface IAuthService
{
    Task<IdentityResult> RegisterUserAsync(RegisterDto userRegisterDto);
    Task<string?> SignInUserAsync(LoginDto userLoginDto);
    Task<string?> GoogleCallbackAsync(ExternalLoginInfo info);
    string GenerateJwtToken(User user);
}