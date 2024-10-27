using UserService.Models;

namespace UserService.Services;

public interface IAuthService
{
    string GenerateJwtToken(User user);
    Task<string> SignInWithGoogle(string googleToken);
}