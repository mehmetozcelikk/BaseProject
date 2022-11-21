using BaseProject.Application.DTOs.OperationClaims;
using BaseProject.Application.DTOs.Page;
using BaseProject.Application.DTOs.UserOperationClaims;
using BaseProject.Application.Repositories;
using BaseProject.Domain.Entities;

namespace BaseProject.Application.Abstractions.Services;

public interface IOperationClaimService 
{
    public Task<OperationClaimDto> GetById(OperationClaimDto request);
    public Task<OperationClaimListDto> GetList(PageRequest request);
    public Task<CreatedOperationClaimDto> Add(CreatedOperationClaimDto request);
    public Task<DeletedOperationClaimDto> Delete(DeletedOperationClaimDto request);
    public Task<UpdatedOperationClaimDto> Update(UpdatedOperationClaimDto request);

}
