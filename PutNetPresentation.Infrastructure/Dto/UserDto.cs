using PutNetPresentation.Infrastructure.Dto.Abstractions;

namespace PutNetPresentation.Infrastructure.Dto
{
    public class UserDto : IDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
