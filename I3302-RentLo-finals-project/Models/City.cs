using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace I3302_RentLo_finals_project.Models
{
    public class City
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string CityName { get; set; }
        [Required]
        [ForeignKey("Country")]
        public int CountryId { get; set; }
        [Required]
        public Country Country { get; set; }
    }
}
