using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandConquestDB.Entities
{
    public class CountryStorage
    {
        [Required]
        [Column("country_id")]
        [StringLength(16)]
        public string CountryId { get; set; }

        [Column("wood")]
        public int CountryWood { get; set; }

        [Column("stone")]
        public int CountryStone { get; set; }

        [Column("food")]
        public int CountryFood { get; set; }

        [Column("iron")]
        public int CountryIron { get; set; }

        [Column("gold_ore")]
        public int CountryGoldOre { get; set; }

        [Column("copper")]
        public int CountryCopper { get; set; }

        [Column("gems")]
        public int CountryGems { get; set; }

        [Column("leather")]
        public int CountryLeather { get; set; }
    }
}
