using BaseProject.Application.Abstractions.Services;
using BaseProject.Application.DTOs.OperationClaims;
using BaseProject.Application.DTOs.Page;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;

namespace BaseProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationClaimsController : ControllerBase
    {

        private readonly IOperationClaimService _operationClaimService;

        public OperationClaimsController(IOperationClaimService operationClaimService)
        {
            _operationClaimService = operationClaimService;
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById([FromRoute] OperationClaimDto getByIdOperationClaimQuery)
        {
            var result = await _operationClaimService.GetById(getByIdOperationClaimQuery);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
        {
            var result = await _operationClaimService.GetList(pageRequest);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreatedOperationClaimDto request)
        {
            var result = await _operationClaimService.Add(request);
            return Created("", result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdatedOperationClaimDto request)
        {
            var result = await _operationClaimService.Update(request);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeletedOperationClaimDto request)
        {
            var result = await _operationClaimService.Delete(request);
            return Ok(result);
        }

    }
}
