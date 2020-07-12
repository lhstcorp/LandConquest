using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandConquest.Entities
{
    public class Country
    {
        [Required]
        [Column("country_id")]
        public int CountryId { get; set; }

        [Required]
        [Column("country_name")]
        [StringLength(22)]
        public string CountryName { get; set; }

        [Required]
        [Column("country_ruler")]
        [StringLength(16)]
        public string CountryRuler { get; set; }

        [Required]
        [Column("country_color")]
        [StringLength(9)]
        public string CountryColor { get; set; }



    }
}
