using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IdentityProvider.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; internal set; }
    }
}
