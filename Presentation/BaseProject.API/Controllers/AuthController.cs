using BaseProject.Application.DTOs;
using CorePackages.Security.Dtos;
using CorePackages.Security.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BaseProject.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : BaseController
{


[HttpPost("Register")]
public async Task<IActionResult> Register([FromBody] UserForRegisterDto userForRegisterDto)
{
    //RegisterCommand registerCommand = new()
    //{
    //    UserForRegisterDto = userForRegisterDto,
    //    IpAddress = GetIpAddress()
    //};

    //RegisteredDto result = await Mediator.Send(registerCommand);
    //SetRefreshTokenToCookie(result.RefreshToken);
    //return Created("", result.AccessToken);
    return Ok( );
}

private void SetRefreshTokenToCookie(RefreshToken refreshToken)
{
    CookieOptions cookieOptions = new() { HttpOnly = true, Expires = DateTime.Now.AddDays(7) };
    Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
}

}