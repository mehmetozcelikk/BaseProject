using BaseProject.Application.Abstractions.Services;
using BaseProject.Application.DTOs.Page;
using BaseProject.Application.DTOs.User;
using BaseProject.Application.Paging;
using BaseProject.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaseProject.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : BaseController
{

    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] UserForRegisterDto userForRegisterDto)
    {

        UserRegisterDTO registerDTO = new UserRegisterDTO();
        registerDTO.userForRegisterDto = userForRegisterDto;
        registerDTO.IpAdress = GetIpAddress();
        var response = await _userService.UserRegister(registerDTO);
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
    [HttpPost("GetAllUsers") ,Authorize()]
    public async Task<IActionResult> GetAllUsers([FromBody] PageRequest pageRequest)
    {
       var aaa = await _userService.GetUsers(pageRequest);


        return Ok( aaa);
    }
}