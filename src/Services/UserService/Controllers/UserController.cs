using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserService.Models;
using UserService.Models.Dtos;
using UserService.Services;

namespace UserService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IAuthService _authService;
    private readonly ILogger<UserController> _logger;
    private readonly IMapper _mapper;

    public UserController(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IAuthService authService,
        ILogger<UserController> logger,
        IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _authService = authService;
        _logger = logger;
        _mapper = mapper;
    }

    [Authorize]
    [HttpGet("Check")]
    public IActionResult CheckAuth()
    {
        return Ok("Auth Working");
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register(RegisterDto userRegisterDto)
    {
        var result = await _authService.RegisterUserAsync(userRegisterDto);
        if (result.Succeeded)
        {
            return Ok();
        }
        return BadRequest(result.Errors);
    }

    [HttpPost("SignIn")]
    public async Task<IActionResult> SignIn(LoginDto userLoginDto)
    {
        string? token = await _authService.SignInUserAsync(userLoginDto);
        if (token != null)
        {
            return Ok(token);
        }
        return BadRequest("Invalid email or password.");
    }

    [HttpGet("SignInWithGoogle")]
    public IActionResult SignInWithGoogle()
    {
        var redirectUrl = Url.Action("GoogleCallback", "User");
        var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
        return Challenge(properties, "Google");
    }

    [HttpGet("GoogleCallback")]
    public async Task<IActionResult> GoogleCallback()
    {
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            return RedirectToAction(nameof(SignIn));
        }

        string? token = await _authService.GoogleCallbackAsync(info);
        if (token != null)
        {
            return Ok(token);
        }
        return BadRequest("Failed to sign in with Google.");
    }

}
