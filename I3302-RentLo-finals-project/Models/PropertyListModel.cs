
using Microsoft.AspNetCore.Mvc.Rendering;

namespace I3302_RentLo_finals_project.Models
{
    public class PropertyListModel
    {
        public List<Property> Properties { get; set; }

        public List<Category> Categories { get; set; }
        public Category PropertyCategory { get; set; }

        public List<Country> Countries { get; set; }
        public Country PropertyCountry { get; set; }

        public List<City> Cities { get; set; }
        public City PropertyCity { get; set; }
        
    }
}
