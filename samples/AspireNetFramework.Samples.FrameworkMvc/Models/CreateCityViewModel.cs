using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

using AspireNetFramework.Samples.DataLibrary.Models;

namespace AspireNetFramework.Samples.FrameworkMvc.Models
{
    public class CreateCityViewModel
    {
        [Display(Name = "City Name")]
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        [Display(Name = "Country")]
        public int SelectedCountryId { get; set; }
        public DbSet<Country> Countries { get; set; }
    }
}