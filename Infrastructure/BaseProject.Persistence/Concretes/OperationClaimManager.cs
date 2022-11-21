using AutoMapper;
using BaseProject.Application.Abstractions.Services;
using BaseProject.Application.Constants;
using BaseProject.Application.DTOs.OperationClaims;
using BaseProject.Application.DTOs.Page;
using BaseProject.Application.Paging;
using BaseProject.Application.Repositories;
using BaseProject.Application.Repositories.UnitOfWork;
using BaseProject.Domain.Entities;
using BaseProject.Persistence.Contexts;
using CorePackages.CrossCuttingConcerns.Exceptions;

namespace BaseProject.Persistence.Concretes;

public class OperationClaimManager : IOperationClaimService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public OperationClaimManager(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }



    public async Task<OperationClaimDto> GetById(OperationClaimDto request)
    {
        OperationClaim? result = await _unitOfWork.operationClaimRepository.GetAsync(b => b.Id == request.Id);
        if (result == null) throw new BusinessException(OperationClaimMessages.OperationClaimNotExists);

        OperationClaim? operationClaim = await _unitOfWork.operationClaimRepository.GetAsync(b => b.Id == request.Id);
        OperationClaimDto operationClaimDto = _mapper.Map<OperationClaimDto>(operationClaim);
        return operationClaimDto;
    }

    public async Task<OperationClaimListDto> GetList(PageRequest request)
    {
        IPaginate<OperationClaim> operationClaims = await _unitOfWork.operationClaimRepository.GetListAsync(
                                                        index: request.Page,
                                                        size: request.PageSize);
        OperationClaimListDto mappedOperationClaimListModel =
            _mapper.Map<OperationClaimListDto>(operationClaims);
        return mappedOperationClaimListModel;
    }



    public async Task<CreatedOperationClaimDto> Add(CreatedOperationClaimDto request)
    {
        OperationClaim mappedOperationClaim = _mapper.Map<OperationClaim>(request);
        OperationClaim createdOperationClaim = await _unitOfWork.operationClaimRepository.AddAsync(mappedOperationClaim);
        CreatedOperationClaimDto createdOperationClaimDto =
            _mapper.Map<CreatedOperationClaimDto>(createdOperationClaim);
        return createdOperationClaimDto;
    }



    public async Task<DeletedOperationClaimDto> Delete(DeletedOperationClaimDto request)
    {
        OperationClaim? result = await _unitOfWork.operationClaimRepository.GetAsync(b => b.Id == request.Id);
        if (result == null) throw new BusinessException(OperationClaimMessages.OperationClaimNotExists);

        OperationClaim mappedOperationClaim = _mapper.Map<OperationClaim>(request);
        OperationClaim deletedOperationClaim = await _unitOfWork.operationClaimRepository.DeleteAsync(mappedOperationClaim);
        DeletedOperationClaimDto deletedOperationClaimDto =
            _mapper.Map<DeletedOperationClaimDto>(deletedOperationClaim);
        return deletedOperationClaimDto;
    }


    public async Task<UpdatedOperationClaimDto> Update(UpdatedOperationClaimDto request)
    {
        OperationClaim mappedOperationClaim = _mapper.Map<OperationClaim>(request);
        OperationClaim updatedOperationClaim = await _unitOfWork.operationClaimRepository.UpdateAsync(mappedOperationClaim);
        UpdatedOperationClaimDto updatedOperationClaimDto =
            _mapper.Map<UpdatedOperationClaimDto>(updatedOperationClaim);
        return updatedOperationClaimDto;
    }


}
