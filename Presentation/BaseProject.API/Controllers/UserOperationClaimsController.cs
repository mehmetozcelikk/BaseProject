using BaseProject.Application.Abstractions.Services;
using BaseProject.Application.DTOs.Page;
using BaseProject.Application.DTOs.UserOperationClaims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BaseProject.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserOperationClaimsController : BaseController
{
    private readonly IUserOperationClaimService _userOperationClaimService;

    public UserOperationClaimsController(IUserOperationClaimService userOperationClaimService)
    {
        _userOperationClaimService = userOperationClaimService;
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> GetById([FromRoute] UserOperationClaimDto request)
    {
        UserOperationClaimDto result = await _userOperationClaimService.GetById(request);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListUserOperationClaimQuery getListUserOperationClaimQuery = new() { PageRequest = pageRequest };
        UserOperationClaimListModel result = await _userOperationClaimService.GetList(getListUserOperationClaimQuery);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreatedUserOperationClaimDto request)
    {
        CreatedUserOperationClaimDto result = await _userOperationClaimService.Add(request);
        return Created("", result);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdatedUserOperationClaimDto request)
    {
        UpdatedUserOperationClaimDto result = await _userOperationClaimService.Update(request);
        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeletedUserOperationClaimDto request)
    {
        DeletedUserOperationClaimDto result = await _userOperationClaimService.Delete(request);
        return Ok(result);
    }
}
