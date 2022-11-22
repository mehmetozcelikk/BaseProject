using AutoMapper;
using BaseProject.Application.Repositories.UnitOfWork;

namespace BaseProject.Application.Abstractions.Services;

public interface IBaseInterface1
{
    IUnitOfWork _unitOfWork { get; }
    IMapper _mapper { get; }
}
