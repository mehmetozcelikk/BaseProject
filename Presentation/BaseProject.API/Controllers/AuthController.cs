﻿using BaseProject.Application.Abstractions.Services;
using BaseProject.Application.DTOs.User;
using BaseProject.Domain.Entities;
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
        //RegisterCommand registerCommand = new()
        //{
        //    UserForRegisterDto = userForRegisterDto,
        //    IpAddress = GetIpAddress()
        //};
        UserRegisterDTO registerDTO = new UserRegisterDTO();
        registerDTO.Email = userForRegisterDto.Email;
        registerDTO.Password = userForRegisterDto.Password;
        registerDTO.FirstName = userForRegisterDto.FirstName;
        registerDTO.LastName = userForRegisterDto.FirstName;
        registerDTO.IpAdress = GetIpAddress();
        var response = await _userService.UserRegister(registerDTO);
        //RegisteredDto result = await Mediator.Send(registerCommand);
        //SetRefreshTokenToCookie(result.RefreshToken);
        //return Created("", result.AccessToken);
        return Ok();
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

}