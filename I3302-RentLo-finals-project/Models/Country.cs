using System.ComponentModel.DataAnnotations;

namespace I3302_RentLo_finals_project.Models
{
    public class Country
    {
        [Key]
        public int id { get; set; }
        [Required]
        [RegularExpression(@"^[A-Za-z]{3}$", ErrorMessage = "ISO code cannot be longer or shorter than 3 alphabetic characters.")]
        public string ISOCode { get; set; }
        [Required]
        public string CountryName { get; set; }
        public ICollection<City> Cities { get; set;}
    }
}
