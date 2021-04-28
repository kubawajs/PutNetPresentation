using Microsoft.AspNetCore.Identity;

namespace PutNetPresentation.Core.Models
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
