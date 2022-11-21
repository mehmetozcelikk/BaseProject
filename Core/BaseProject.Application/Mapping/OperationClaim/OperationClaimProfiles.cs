using AutoMapper;
using BaseProject.Application.DTOs.OperationClaims;
using BaseProject.Application.DTOs.UserOperationClaims;
using BaseProject.Application.Paging;
using BaseProject.Domain.Entities;

namespace BaseProject.Application.Mapping.OperationClaimProfiles
{
    public class OperationClaimProfiles : Profile
    {
        public OperationClaimProfiles()
        {
            CreateMap<OperationClaim, CreatedOperationClaimDto>().ReverseMap();
            CreateMap<OperationClaim, UpdatedOperationClaimDto>().ReverseMap();
            CreateMap<OperationClaim, DeletedOperationClaimDto>().ReverseMap();
            CreateMap<OperationClaim, OperationClaimDto>().ReverseMap();
            CreateMap<OperationClaim, OperationClaimListDto>().ReverseMap();
            CreateMap<IPaginate<OperationClaim>, OperationClaimListDto>().ReverseMap();

        }

    }
}
