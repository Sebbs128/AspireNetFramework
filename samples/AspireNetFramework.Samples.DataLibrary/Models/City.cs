using System.ComponentModel.DataAnnotations.Schema;

namespace AspireNetFramework.Samples.DataLibrary.Models
{
    public class City
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        [ForeignKey(nameof(Country))]
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }
    }
}
