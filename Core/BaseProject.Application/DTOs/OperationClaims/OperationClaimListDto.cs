using BaseProject.Application.Paging;

namespace BaseProject.Application.DTOs.OperationClaims;

public class OperationClaimListDto : BasePageableModel
{
    public IList<OperationClaimDto> Items { get; set; }
}
