using Microsoft.AspNetCore.Identity;

namespace api.Models
{
    public class AppUser : IdentityUser
    {
        // Navigation Properties
        public List<Portfolio> Portfolios { get; set; } = new List<Portfolio>();
        public List<Comment> Comment { get; set; } = new List<Comment>();
    }
}