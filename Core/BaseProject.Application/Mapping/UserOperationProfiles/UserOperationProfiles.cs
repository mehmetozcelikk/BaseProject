using AutoMapper;
using BaseProject.Application.DTOs.UserOperationClaims;
using BaseProject.Application.Paging;
using BaseProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseProject.Application.Mapping.UserOperationProfiles
{
    public class UserOperationProfiles : Profile
    {
        public UserOperationProfiles()
        {
            CreateMap<UserOperationClaim, CreatedUserOperationClaimDto>().ReverseMap();
            CreateMap<UserOperationClaim, UpdatedUserOperationClaimDto>().ReverseMap();
            CreateMap<UserOperationClaim, DeletedUserOperationClaimDto>().ReverseMap();
            CreateMap<UserOperationClaim, UserOperationClaimDto>().ReverseMap();
            CreateMap<UserOperationClaim, UserOperationClaimListDto>().ReverseMap();
            CreateMap<IPaginate<UserOperationClaim>, UserOperationClaimDto>().ReverseMap();
        }
    }
}
