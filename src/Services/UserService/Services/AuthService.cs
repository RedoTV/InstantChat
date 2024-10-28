using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UserService.Data;
using UserService.Models;
using UserService.Models.Dtos;

namespace UserService.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration, IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _mapper = mapper;
    }

    public async Task<IdentityResult> RegisterUserAsync(RegisterDto userRegisterDto)
    {
        var user = _mapper.Map<User>(userRegisterDto);
        return await _userManager.CreateAsync(user, userRegisterDto.Password);
    }

    public async Task<string?> SignInUserAsync(LoginDto userLoginDto)
    {
        var user = await _userManager.FindByEmailAsync(userLoginDto.Email);
        if (user == null)
            return null;

        var result = await _signInManager.PasswordSignInAsync(user, userLoginDto.Password, false, false);
        if (result.Succeeded)
            return GenerateJwtToken(user);

        return null;
    }

    public async Task<string?> GoogleCallbackAsync(ExternalLoginInfo info)
    {
        var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
        if (result.Succeeded)
        {
            var user = await _userManager.FindByEmailAsync(info.Principal.FindFirstValue(ClaimTypes.Email)!);
            return GenerateJwtToken(user!);
        }

        var newUser = new User
        {
            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
            Email = info.Principal.FindFirstValue(ClaimTypes.Email)
        };

        var createUserResult = await _userManager.CreateAsync(newUser);
        if (!createUserResult.Succeeded)
            return null;

        await _userManager.AddLoginAsync(newUser, info);
        return GenerateJwtToken(newUser);
    }

    public string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email!)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}
