using System.ComponentModel.DataAnnotations;

namespace I3302_RentLo_finals_project.Models
{
    public class UserPropertyRent
    {
        [Key]
        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        public Property Property { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
