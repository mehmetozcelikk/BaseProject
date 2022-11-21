using BaseProject.Application.Paging;

namespace BaseProject.Application.DTOs.UserOperationClaims;

public class UserOperationClaimListDto: BasePageableModel
{
    public IList<UserOperationClaimDto> Items { get; set; }

}