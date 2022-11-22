using BaseProject.Application.Abstractions.Services;
using BaseProject.Application.DTOs.Auth;
using BaseProject.Application.DTOs.Page;
using BaseProject.Application.DTOs.User;
using BaseProject.Domain.Entities;
using CorePackages.Security.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BaseProject.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : BaseController
{

    private readonly IUserService _userService;
    private readonly IAuthService _authService;
    public AuthController(IUserService userService, IAuthService authService = null)
    {
        _userService = userService;
        _authService = authService;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] UserForRegisterDto userForRegisterDto)
    {

        UserRegisterDTO registerDTO = new UserRegisterDTO();
        registerDTO.userForRegisterDto = userForRegisterDto;
        registerDTO.IpAdress = GetIpAddress();
        var response = await _authService.Register(registerDTO);
        SetRefreshTokenToCookie(response.RefreshToken);
        return Created("", response.AccessToken);
    }
    protected string? GetIpAddress()
    {
        if (Request.Headers.ContainsKey("X-Forwarded-For")) return Request.Headers["X-Forwarded-For"];
        return HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
    }
    private void SetRefreshTokenToCookie(RefreshToken refreshToken)
    {
        CookieOptions cookieOptions = new() { HttpOnly = true, Expires = DateTime.Now.AddDays(7) };
        Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpGet("GetAllUsers")  ]
    public async Task<IActionResult> GetAllUsers([FromQuery] PageRequest pageRequest)
    {
        var aaa = await _userService.GetUsers(pageRequest);
        UserLoginDTO loginDTO = new UserLoginDTO();
        loginDTO.IPAddress = "adsdas";
         
        return Ok(loginDTO);
    }








    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] UserForLoginDto userForLoginDto)
    {
        UserLoginDTO userLogin = new() { userForLoginDto = userForLoginDto, IPAddress = GetIpAddress() };
        var result = await _authService.Login(userLogin);

        if (result.RefreshToken is not null) setRefreshTokenToCookie(result.RefreshToken);

        return Ok(result.CreateResponseDto());
    }

    //[HttpPost("Register")]
    //public async Task<IActionResult> Register([FromBody] UserForRegisterDto userForRegisterDto)
    //{
    //    UserRegisterDTO registerDTO = new() { userForRegisterDto = userForRegisterDto, IpAdress = GetIpAddress() };
    //    var result = await _userService.UserRegister(registerDTO);
    //    setRefreshTokenToCookie(result.RefreshToken);
    //    return Created("", result.AccessToken);
    //}

    [HttpGet("RefreshToken")]
    public async Task<IActionResult> RefreshToken()
    {
        RefreshTokenDTO request = new()
        { RefleshToken = getRefreshTokenFromCookies(), IPAddress = GetIpAddress() };
        RefreshedTokensDto result = await _authService.RefreshToken(request);
        setRefreshTokenToCookie(result.RefreshToken);
        return Created("", result.AccessToken);
    }

    [HttpPut("RevokeToken")]
    public async Task<IActionResult> RevokeToken(
        [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)]
        string? refreshToken)
    {
        RevokeTokenDTO request = new()
        {
            Token = refreshToken ?? getRefreshTokenFromCookies(),
            IPAddress = GetIpAddress()
        };
        RevokeTokenDTO result = await _authService.RevokeToken(request);
        return Ok(result);
    }

    [HttpGet("EnableEmailAuthenticator")]
    public async Task<IActionResult> EnableEmailAuthenticator(EnableEmailAuthenticatorDTO request)
    {

        await _authService.EnableEmailAuthenticator(request);

        return Ok();
    }
    protected int getUserIdFromRequest() //todo authentication behavior?
    {
        int userId = HttpContext.User.GetUserId();
        return userId;
    }

    [HttpGet("EnableOtpAuthenticator")]
    public async Task<IActionResult> EnableOtpAuthenticator()
    {
        EnableOtpAuthenticatorDTO enableOtpAuthenticatorCommand = new()
        {
            UserId = getUserIdFromRequest()
        };
        var result = await _authService.EnableOtpAuthenticator(enableOtpAuthenticatorCommand);

        return Ok(result);
    }

    [HttpGet("VerifyEmailAuthenticator")]
    public async Task<IActionResult> VerifyEmailAuthenticator(
        [FromQuery] VerifyEmailAuthenticatorDTO verifyEmailAuthenticatorCommand)
    {
        await _authService.VerifyEmailAuthenticator(verifyEmailAuthenticatorCommand);
        return Ok();
    }

    [HttpPost("VerifyOtpAuthenticator")]
    public async Task<IActionResult> VerifyOtpAuthenticator(
        [FromBody] string authenticatorCode)
    {
        VerifyOtpAuthenticatorDTO request =
            new() { UserId = getUserIdFromRequest(), ActivationCode = authenticatorCode };

        await _authService.VerifyOtpAuthenticator(request);
        return Ok();
    }

    private string? getRefreshTokenFromCookies()
    {
        return Request.Cookies["refreshToken"];
    }

    private void setRefreshTokenToCookie(RefreshToken refreshToken)
    {
        CookieOptions cookieOptions = new() { HttpOnly = true, Expires = DateTime.UtcNow.AddDays(7) };
        Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
    }













}