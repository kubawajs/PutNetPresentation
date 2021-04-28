using PutNetPresentation.Infrastructure.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PutNetPresentation.Infrastructure.Services.Abstractions
{
    public interface IUserService : IService
    {
        Task CreateUserAsync(RegisterUserDto registerUserDto);
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<IEnumerable<UserDto>> GetAllAsync(int pageNumber, int pageSize);
        Task<UserDto> GetAsync(string username);
    }
}