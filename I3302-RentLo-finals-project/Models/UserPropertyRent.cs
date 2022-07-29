using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace I3302_RentLo_finals_project.Models
{
    public class UserPropertyRent
    {
        [Key]
        public int Id { get; set; }
        
        [ForeignKey("AspNetUsers")]
        [Required]
        public string? RenterId { get; set; }
        public ApplicationUser? Renter { get; set; }

        [ForeignKey("Property")]
        [Required]
        
        public int PropertyId { get; set; }
        public Property? Property { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;

/*        [Remote(action: "VerifyDateRangeAvailability", controller: "UserPropertyRents", AdditionalFields = nameof(PropertyId) + "," + nameof(StartDate) + "," + nameof(EndDate))]
        [Display(Name = "Check In Date")]*/
        public DateTime StartDate { get; set; }

        [Remote(action: "VerifyDateRangeAvailability", controller: "UserPropertyRents", AdditionalFields = nameof(PropertyId) + "," + nameof(StartDate) + "," + nameof(EndDate))]
        [Display(Name = "Check Out Date")]
        public DateTime EndDate { get; set; }

        [NotMapped]
        public string state { get; set; } = "up-coming";
        [NotMapped]
        public double totalNumberOfDays { get; set; }
        [NotMapped]
        public double totalPrice { get; set; }

    }
}
