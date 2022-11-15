﻿using BaseProject.Application.Abstractions.Services;
using BaseProject.Application.DTOs;
using BaseProject.Application.DTOs.User;
using BaseProject.Application.Repositories.EntityRepositories;
using CorePackages.Security.Dtos;
using CorePackages.Security.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserForRegisterDto = CorePackages.Security.Dtos.UserForRegisterDto;

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
        var response = _userService.UserRegister(registerDTO);
        //RegisteredDto result = await Mediator.Send(registerCommand);
        //SetRefreshTokenToCookie(result.RefreshToken);
        //return Created("", result.AccessToken);
        return Ok();
    }

    private void SetRefreshTokenToCookie(RefreshToken refreshToken)
    {
        CookieOptions cookieOptions = new() { HttpOnly = true, Expires = DateTime.Now.AddDays(7) };
        Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
    }

}