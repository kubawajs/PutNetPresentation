using AutoMapper;
using PutNetPresentation.Core.Repositories;
using PutNetPresentation.Infrastructure.Dto;
using PutNetPresentation.Infrastructure.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PutNetPresentation.Core.Models;

namespace PutNetPresentation.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task CreateUserAsync(RegisterUserDto registerUserDto)
        {
            var user = await _userRepository.GetAsync(registerUserDto.Username);
            if (user != null)
            {
                throw new Exception($"User with email: '{registerUserDto.Email}' already exists.");
            }

            user = _mapper.Map<User>(registerUserDto);
            await _userRepository.CreateAsync(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            var users = await _userRepository.GetAllAsync(pageNumber, pageSize);
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto> GetAsync(string username)
        {
            var user = await _userRepository.GetAsync(username);
            return _mapper.Map<UserDto>(user);
        }
    }
}
