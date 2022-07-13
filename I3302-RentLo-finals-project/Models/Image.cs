using System.ComponentModel.DataAnnotations.Schema;

namespace I3302_RentLo_finals_project.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }

        [ForeignKey("Property")]
        public int PropertyId { get; set; }
        public Property Property { get; set; }
    }
}
