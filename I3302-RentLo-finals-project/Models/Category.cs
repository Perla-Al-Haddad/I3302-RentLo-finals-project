using System.ComponentModel.DataAnnotations;

namespace I3302_RentLo_finals_project.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string CategoryDescription { get; set; }
    }
} 
