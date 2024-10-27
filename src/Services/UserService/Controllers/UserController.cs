using AutoMapper;
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

    [HttpPost("Register")]
    public async Task<IActionResult> Register(RegisterDto userRegisterDto)
    {
        User user = _mapper.Map<User>(userRegisterDto);
        var result = await _userManager.CreateAsync(user, userRegisterDto.Password);

        if (result.Succeeded)
        {
            return Ok();
        }

        return BadRequest(result.Errors);
    }

    [HttpPost("SignIn")]
    public async Task<IActionResult> SignIn(LoginDto userLoginDto)
    {
        try
        {
            User? user = await _userManager.FindByEmailAsync(userLoginDto.Email);
            if (user == null)
            {
                return BadRequest("Invalid email or password.");
            }

            var result = await _signInManager.PasswordSignInAsync(user, userLoginDto.Password, false, false);
            if (result.Succeeded)
            {
                string token = _authService.GenerateJwtToken(user);
                return Ok(token);
            }

            return BadRequest("Invalid email or password.");
        }
        catch (Exception exc)
        {
            _logger.LogError(exc, exc.Message);
            return BadRequest(exc.Message);
        }
    }

    [HttpPost("SignInWithGoogle")]
    public async Task<IActionResult> SignInWithGoogle([FromBody] string googleToken)
    {
        try
        {
            string token = await _authService.SignInWithGoogle(googleToken);
            return Ok(token);
        }
        catch (Exception exc)
        {
            _logger.LogError(exc, exc.Message);
            return BadRequest(exc.Message);
        }
    }
}
