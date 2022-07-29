using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace I3302_RentLo_finals_project.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
    }
}
