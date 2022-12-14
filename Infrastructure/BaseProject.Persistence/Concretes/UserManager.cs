using BaseProject.Application.Abstractions.Services;
using BaseProject.Application.DTOs;
using BaseProject.Application.DTOs.Page;
using BaseProject.Application.DTOs.User;
using BaseProject.Application.Paging;
using BaseProject.Application.Repositories.UnitOfWork;
using BaseProject.Domain.Entities;
using CorePackages.CrossCuttingConcerns.Exceptions;
using CorePackages.Security.Hashing;
using CorePackages.Security.JWT;

namespace BaseProject.Persistence.Concretes
{
    public class UserManager : IUserService
    {

        private readonly IUnitOfWork _unitOfWork;

        public async Task<IPaginate<User>> GetUsers(PageRequest pageRequest)
        {
            var users = await _unitOfWork.userRepository.GetListAsync(index: pageRequest.Page, size: pageRequest.PageSize);
            return users;
        }

        public UserManager( IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //public async Task<RegisteredDto> UserRegister(UserRegisterDTO request)
        //{
        //    byte[] passwordHash, passwordSalt;
        //    HashingHelper.CreatePasswordHash(request.userForRegisterDto.Password, out passwordHash, out passwordSalt);

        //    User? user = await _unitOfWork.userRepository.GetAsync(u => u.Email == request.userForRegisterDto.Email);
        //    if (user != null) throw new BusinessException("Mail already exists");


        //    var count = _unitOfWork.userRepository.GetList();
        //    User newUser = new()
        //    {
        //        Email = request.userForRegisterDto.Email,
        //        PasswordHash = passwordHash,
        //        PasswordSalt = passwordSalt,
        //        FirstName = request.userForRegisterDto.FirstName,
        //        LastName = request.userForRegisterDto.LastName,
        //        Status = true
        //    };
        //    User createdUser = _unitOfWork.userRepository.AddAsync(newUser).Result;

        //    AccessToken createdAccessToken = await _authService.CreateAccessToken(createdUser);
        //    RefreshToken createdRefreshToken = await _authService.CreateRefreshToken(createdUser, request.IpAdress);
        //    RefreshToken addedRefreshToken = await _authService.AddRefreshToken(createdRefreshToken);

        //    RegisteredDto registeredDto = new()
        //    {
        //        RefreshToken = addedRefreshToken,
        //        AccessToken = createdAccessToken,
        //    };
        //    return registeredDto;

        //}


        public async Task<User?> GetByEmail(string email)
        {
            User? user = await _unitOfWork.userRepository.GetAsync(u => u.Email == email);
            return user;
        }

        public async Task<User> GetById(int id)
        {
            User? user = await _unitOfWork.userRepository.GetAsync(u => u.Id == id);
            return user;
        }

        public async Task<User> Update(User user)
        {
            User updatedUser = await _unitOfWork.userRepository.UpdateAsync(user);
            return updatedUser;
        }








    }

}
