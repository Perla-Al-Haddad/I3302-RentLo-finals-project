using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace I3302_RentLo_finals_project.Models
{
    public class Property
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string PropertyTitle { get; set; }
        public string PropertyDescription { get; set; }
        [Required]
        public double PricePerDay { get; set; }
        [Required]
        public DateTime DateCreated { get; set; } = DateTime.Now;
        [Required]
        public int MaxGuests { get; set; }
        [Required]
        public int NumberOfBeds { get; set; }
        [Required]
        public int NumberOfBathrooms { get; set; }
        public int Favorites { get; set; }
        
        [Required]
        public ApplicationUser Creator { get; set; }
        [Required]
        public Category Category { get; set; }
        [Required]
        public City City { get; set; }
    }
}
