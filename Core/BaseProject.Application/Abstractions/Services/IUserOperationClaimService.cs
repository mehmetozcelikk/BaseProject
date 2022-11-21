using BaseProject.Application.DTOs.Page;
using BaseProject.Application.DTOs.UserOperationClaims;

namespace BaseProject.Application.Abstractions.Services;

public interface IUserOperationClaimService
{
    public Task<UserOperationClaimDto> GetById(UserOperationClaimDto request);
    public Task<UserOperationClaimListDto> GetList(PageRequest request);
    public Task<CreatedUserOperationClaimDto> Add(CreatedUserOperationClaimDto request);
    public Task<DeletedUserOperationClaimDto> Delete(DeletedUserOperationClaimDto request);
    public Task<UpdatedUserOperationClaimDto> Update(UpdatedUserOperationClaimDto request);


}
