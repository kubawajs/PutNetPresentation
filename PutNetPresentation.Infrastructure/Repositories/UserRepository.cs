using PutNetPresentation.Core.Models;
using PutNetPresentation.Core.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PutNetPresentation.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private static readonly List<User> _users = GenerateStaticUserList();

        private static List<User> GenerateStaticUserList()
        {
            var usersList = new List<User>();
            for (var i = 0; i < 100; i++)
            {
                usersList.Add(new User
                {
                    Id = i,
                    Email = $"user{i}@test.com",
                    UserName = $"username_{i}",
                    FirstName = $"Name_{i}",
                    LastName = $"LastName_{i}"
                });
            }

            return usersList;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
            => await Task.FromResult(_users);

        public async Task<IEnumerable<User>> GetAllAsync(int pageNumber, int pageSize) 
            => await Task.FromResult(_users.OrderBy(x => x.Id).Skip((pageNumber - 1) * pageSize).Take(pageSize));

        public async Task<User> GetAsync(string username) 
            => await Task.FromResult(_users.SingleOrDefault(x => x.UserName == username.ToLowerInvariant()));

        public async Task CreateAsync(User user)
        {
            _users.Add(user);
            await Task.CompletedTask;
        }
	}
}
