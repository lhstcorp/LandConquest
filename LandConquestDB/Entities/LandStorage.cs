using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandConquestDB.Entities
{
    public class LandStorage
    {
        [Required]
        [Column("land_id")]
        [StringLength(16)]
        public string LandId { get; set; }

        [Required]
        [Column("country_ruler")]
        [StringLength(16)]
        public string CountryRuler { get; set; }

        [Column("wood")]
        public int LandWood { get; set; }

        [Column("stone")]
        public int LandStone { get; set; }

        [Column("food")]
        public int LandFood { get; set; }

        [Column("iron")]
        public int LandIron { get; set; }

        [Column("gold_ore")]
        public int LandGoldOre { get; set; }

        [Column("copper")]
        public int LandCopper { get; set; }

        [Column("gems")]
        public int LandGems { get; set; }

        [Column("leather")]
        public int LandLeather { get; set; }
    }
}
