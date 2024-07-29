using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLibrary.Models
{
    public class Country
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<City> Cities { get; set; }
    }
}