using PutNetPresentation.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PutNetPresentation.Core.Repositories
{
    public interface IUserRepository : IRepository
    {
        Task CreateAsync(User user);
        Task<IEnumerable<User>> GetAllAsync();
        Task<IEnumerable<User>> GetAllAsync(int pageNumber, int pageSize);
        Task<User> GetAsync(string username);
    }
}