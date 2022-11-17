using BaseProject.Application.Abstractions.Services;
using BaseProject.Application.DTOs;
using BaseProject.Application.DTOs.User;
using BaseProject.Application.Repositories.EntityRepositories;
using BaseProject.Application.Repositories.UnitOfWork;
using BaseProject.Domain.Entities;
using CorePackages.CrossCuttingConcerns.Exceptions;
using CorePackages.Security.Hashing;
using CorePackages.Security.JWT;

namespace BaseProject.Persistence.Concretes
{
    public class UserManager : IUserService
    {
        public UserRegisterDTO UserRegisterDTO { get; set; }

        IAuthService _authService;
        protected IUnitOfWork _unitOfWork;

        public List<User> GetUsers()
        {
            throw new NotImplementedException();
        }
        public UserForRegisterDto UserForRegisterDto { get; set; }
        public string IpAddress { get; set; }




        public UserManager(IAuthService authService, IUnitOfWork unitOfWork)
        {
            _authService = authService;
            _unitOfWork = unitOfWork;
        }

        public async Task<RegisteredDto> UserRegister(UserRegisterDTO request)
        {
            ////await _authBusinessRules.EmailCanNotBeDuplicatedWhenRegistered(request.UserForRegisterDto.Email);
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(request.Password, out passwordHash, out passwordSalt);
            User? user = await _unitOfWork.userRepository.GetAsync(u => u.Email == request.Email);
            if (user != null) throw new BusinessException("Mail already exists");
            var count = _unitOfWork.userRepository.GetList();
            User newUser = new()
            {
                Email = request.Email,
                //PasswordHash = passwordHash,
                //PasswordSalt = passwordSalt,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Status = true
            };


            User createdUser = _unitOfWork.userRepository.AddAsync(newUser).Result;


            AccessToken createdAccessToken = await _authService.CreateAccessToken(createdUser);
            RefreshToken createdRefreshToken =  await _authService.CreateRefreshToken(createdUser, request.IpAdress);
            RefreshToken addedRefreshToken = await _authService.AddRefreshToken(createdRefreshToken);

            RegisteredDto registeredDto = new()
            {
                //RefreshToken = addedRefreshToken,
                //AccessToken = createdAccessToken,
            };
            return registeredDto;

        }
    }

}
