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
        [Display(Name = "Property Title")]
        public string? PropertyTitle { get; set; }
        [Display(Name = "Description")]
        public string? PropertyDescription { get; set; }
        [Required]
        [Display(Name = "Price per Day")]
        public double PricePerDay { get; set; }
        [Required]
        public DateTime DateCreated { get; set; } = DateTime.Now;
        [Required]
        [Display(Name = "Guests")]
        public int MaxGuests { get; set; }
        [Required]
        [Display(Name = "Beds")]
        public int NumberOfBeds { get; set; }
        [Required]
        [Display(Name = "Bathrooms")]
        public int NumberOfBathrooms { get; set; }
        public int Favorites { get; set; }
        [NotMapped]
        public ICollection<IFormFile> ImagePaths { get; set; }
        public ICollection<Image> Images { get; set; }
        [NotMapped]
        public Image MainImage { get; set; }


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
