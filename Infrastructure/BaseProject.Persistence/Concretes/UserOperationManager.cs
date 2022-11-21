using AutoMapper;
using BaseProject.Application.Abstractions.Services;
using BaseProject.Application.Constants;
using BaseProject.Application.DTOs.Page;
using BaseProject.Application.DTOs.UserOperationClaims;
using BaseProject.Application.Paging;
using BaseProject.Application.Repositories;
using BaseProject.Application.Repositories.EntityRepositories;
using BaseProject.Application.Repositories.UnitOfWork;
using BaseProject.Domain.Entities;
using BaseProject.Persistence.Contexts;
using CorePackages.CrossCuttingConcerns.Exceptions;

namespace BaseProject.Persistence.Concretes;

public class UserOperationManager : IUserOperationClaimService
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UserOperationManager(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UserOperationClaimDto> GetById(UserOperationClaimDto request
                                                     )
    {
        UserOperationClaim? result = await _unitOfWork.userOperationClaimRepository.GetAsync(b => b.Id == request.Id);
        if (result == null) throw new BusinessException(UserOperationClaimMessages.UserOperationClaimNotExists);
        UserOperationClaim? userOperationClaim =
            await _unitOfWork.userOperationClaimRepository.GetAsync(b => b.Id == request.Id);
        UserOperationClaimDto userOperationClaimDto = _mapper.Map<UserOperationClaimDto>(userOperationClaim);
        return userOperationClaimDto;
    }

    public async Task<UserOperationClaimListDto> GetList(PageRequest request
                                                           )
    {
        IPaginate<UserOperationClaim> userOperationClaims = await _unitOfWork.userOperationClaimRepository.GetListAsync(
                                                                index: request.Page,
                                                                size: request.PageSize);
        UserOperationClaimListDto mappedUserOperationClaimListModel =
            _mapper.Map<UserOperationClaimListDto>(userOperationClaims);

        return mappedUserOperationClaimListModel;
    }


    public async Task<CreatedUserOperationClaimDto> Add(CreatedUserOperationClaimDto request
                                                            )
    {
        UserOperationClaim mappedUserOperationClaim = _mapper.Map<UserOperationClaim>(request);
        UserOperationClaim createdUserOperationClaim =
            await _unitOfWork.userOperationClaimRepository.AddAsync(mappedUserOperationClaim);
        CreatedUserOperationClaimDto createdUserOperationClaimDto =
            _mapper.Map<CreatedUserOperationClaimDto>(createdUserOperationClaim);
        return createdUserOperationClaimDto;
    }

    public async Task<DeletedUserOperationClaimDto> Delete(DeletedUserOperationClaimDto request
                                                            )
    {
        UserOperationClaim? result = await _unitOfWork.userOperationClaimRepository.GetAsync(b => b.Id == request.Id);
        if (result == null) throw new BusinessException(UserOperationClaimMessages.UserOperationClaimNotExists);

        UserOperationClaim mappedUserOperationClaim = _mapper.Map<UserOperationClaim>(request);
        UserOperationClaim deletedUserOperationClaim =
            await _unitOfWork.userOperationClaimRepository.DeleteAsync(mappedUserOperationClaim);
        DeletedUserOperationClaimDto deletedUserOperationClaimDto =
            _mapper.Map<DeletedUserOperationClaimDto>(deletedUserOperationClaim);
        return deletedUserOperationClaimDto;
    }

    public async Task<UpdatedUserOperationClaimDto> Update(UpdatedUserOperationClaimDto request
                                                            )
    {
        UserOperationClaim mappedUserOperationClaim = _mapper.Map<UserOperationClaim>(request);
        UserOperationClaim updatedUserOperationClaim =
            await _unitOfWork.userOperationClaimRepository.UpdateAsync(mappedUserOperationClaim);
        UpdatedUserOperationClaimDto updatedUserOperationClaimDto =
            _mapper.Map<UpdatedUserOperationClaimDto>(updatedUserOperationClaim);
        return updatedUserOperationClaimDto;
    }


}

