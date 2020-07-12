using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LandConquest.Entities
{
    public class PlayerStorage
    {
        [Required]
        [Column("player_id")]
        [StringLength(16)]
        public string PlayerId { get; set; }

        [Column("wood")]
        public int PlayerWood { get; set; }

        [Column("stone")]
        public int PlayerStone { get; set; }

        [Column("food")]
        public int PlayerFood { get; set; }

        [Column("iron")]
        public int PlayerIron { get; set; }

        [Column("gold_ore")]
        public int PlayerGoldOre { get; set; }

        [Column("copper")]
        public int PlayerCopper { get; set; }

        [Column("gems")]
        public int PlayerGems { get; set; }

        [Column("leather")]
        public int PlayerLeather { get; set; }
    }
}
