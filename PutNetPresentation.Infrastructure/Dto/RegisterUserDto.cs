using PutNetPresentation.Infrastructure.Dto.Abstractions;

namespace PutNetPresentation.Infrastructure.Dto
{
    public class RegisterUserDto : IDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
