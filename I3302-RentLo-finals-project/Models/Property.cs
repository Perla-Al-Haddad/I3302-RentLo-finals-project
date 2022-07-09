using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace I3302_RentLo_finals_project.Models
{
    public class Property
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? PropertyTitle { get; set; }
        public string? PropertyDescription { get; set; }
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
        [ForeignKey("AspNetUsers")]
        public string? CreatorId { get; set; }
        public ApplicationUser? Creator { get; set; }
        
        [Required]
        [ForeignKey("Categories")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        
        [Required]
        [ForeignKey("Cities")]
        public int CityId { get; set; }
        public City? City { get; set; }
    }
}
