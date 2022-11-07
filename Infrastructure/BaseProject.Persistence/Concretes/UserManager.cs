using BaseProject.Application.Abstractions.Services;
using BaseProject.Application.DTOs;
using BaseProject.Application.DTOs.User;
using BaseProject.Application.Repositories.EntityRepositories;
using BaseProject.Domain.Entities;
using CorePackages.Security.Dtos;
using CorePackages.Security.Hashing;
using CorePackages.Security.JWT;

namespace BaseProject.Persistence.Concretes
{
    public class UserManager : IUserService
    {
        public UserRegisterDTO UserRegisterDTO { get; set; }

        IUserService _userService;
        IAuthService _authService;
        private readonly IUserRepository _userRepository;

        public List<User> GetUsers()
        {
            throw new NotImplementedException();
        }
        public UserForRegisterDto UserForRegisterDto { get; set; }
        public string IpAddress { get; set; }


 

        public UserManager(IUserService userService, IAuthService authService, IUserRepository userRepository)
        {
            _userService = userService;
            _authService = authService;
            _userRepository = userRepository;
        }

        public async Task<RegisteredDto> Handle(UserManager request, CancellationToken cancellationToken)
        {
            //await _authBusinessRules.EmailCanNotBeDuplicatedWhenRegistered(request.UserForRegisterDto.Email);
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(request.UserForRegisterDto.Password, out passwordHash, out passwordSalt);
            //User? user = await _userService.GetAsync(u => u.Email == email);
            //if (user != null) throw new BusinessException("Mail already exists");

            User newUser = new()
            {
                Email = request.UserForRegisterDto.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                FirstName = request.UserForRegisterDto.FirstName,
                LastName = request.UserForRegisterDto.LastName,
                Status = true
            };

            User createdUser = await _userRepository.AddAsync(newUser);

            AccessToken createdAccessToken = await _authService.CreateAccessToken(createdUser);
            RefreshToken createdRefreshToken = await _authService.CreateRefreshToken(createdUser, request.IpAddress);
            RefreshToken addedRefreshToken = await _authService.AddRefreshToken(createdRefreshToken);

            RegisteredDto registeredDto = new()
            {
                //RefreshToken = addedRefreshToken,
                AccessToken = createdAccessToken,
            };
            return registeredDto;

        }
    }

}
