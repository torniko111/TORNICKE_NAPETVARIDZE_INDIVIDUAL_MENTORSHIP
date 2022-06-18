using Microsoft.AspNetCore.Identity;

namespace IsRoleDemo.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public bool Subcribed { get; set; }
    }
}
