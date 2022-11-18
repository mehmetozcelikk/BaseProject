using AutoMapper;
using BaseProject.Application.Abstractions.Services;
using BaseProject.Application.DTOs.UserOperationClaims;
using BaseProject.Application.Repositories;
using BaseProject.Application.Repositories.EntityRepositories;
using BaseProject.Domain.Entities;
using BaseProject.Persistence.Contexts;

namespace BaseProject.Persistence.Concretes;

public class UserOperationManager : IUserOperationClaimService
{

    private readonly IUserOperationClaimRepository _userOperationClaimRepository;
    private readonly IMapper _mapper;

    public UserOperationManager(IUserOperationClaimRepository userOperationClaimRepository,
                                                 IMapper mapper
                                                  )
    {
        _userOperationClaimRepository = userOperationClaimRepository;
        _mapper = mapper;
    }


    public async Task<UserOperationClaimDto> Handle(GetByIdUserOperationClaimQuery request,
                                                    CancellationToken cancellationToken)
    {
        await _userOperationClaimBusinessRules.UserOperationClaimIdShouldExistWhenSelected(request.Id);

        UserOperationClaim? userOperationClaim =
            await _userOperationClaimRepository.GetAsync(b => b.Id == request.Id);
        UserOperationClaimDto userOperationClaimDto = _mapper.Map<UserOperationClaimDto>(userOperationClaim);
        return userOperationClaimDto;
    }
}

