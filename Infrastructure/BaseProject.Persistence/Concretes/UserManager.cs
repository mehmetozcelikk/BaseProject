using BaseProject.Application.Abstractions.Services;
using BaseProject.Application.DTOs;
using BaseProject.Application.DTOs.User;
using BaseProject.Application.Repositories.EntityRepositories;
using BaseProject.Application.Repositories.UnitOfWork;
using BaseProject.Domain.Entities;
using CorePackages.Security.Hashing;

namespace BaseProject.Persistence.Concretes
{
    public class UserManager : IUserService
    {
        public UserRegisterDTO UserRegisterDTO { get; set; }

        IAuthService _authService;
        private IUnitOfWork _unitOfWork;

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
            //await _authBusinessRules.EmailCanNotBeDuplicatedWhenRegistered(request.UserForRegisterDto.Email);
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(request.Password, out passwordHash, out passwordSalt);
            //User? user = await _userService.GetAsync(u => u.Email == email);
            //if (user != null) throw new BusinessException("Mail already exists");
            //var count = _unitOfWork.userRepository.GetList();
            User newUser = new()
            {
                Email = request.Email,
                //PasswordHash = passwordHash,
                //PasswordSalt = passwordSalt,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Status = true
            };

            try
            {
                User createdUser2 = await _unitOfWork.userRepository.AddAsync(newUser);

                //var ıd = await _unitOfWork.SaveAsync();
                //createdUser.Id = ıd;

            }
            catch (Exception ex)
            {

                throw;
            }
            User createdUser = await _unitOfWork.userRepository.AddAsync(newUser);

            //AccessToken createdAccessToken = await _authService.CreateAccessToken(createdUser);
            //RefreshToken createdRefreshToken = await _authService.CreateRefreshToken(createdUser, IpAddress);
            //RefreshToken addedRefreshToken = await _authService.AddRefreshToken(createdRefreshToken);

            RegisteredDto registeredDto = new()
            {
                //RefreshToken = addedRefreshToken,
                //AccessToken = createdAccessToken,
            };
            return registeredDto;

        }
    }

}
