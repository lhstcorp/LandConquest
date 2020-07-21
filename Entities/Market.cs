using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandConquest.Entities
{
    public class Market
    {
        [Required]
        [Column("player_id")]
        [StringLength(16)]
        public string PlayerId { get; set; }

        [Column("wood")]
        public int MarketWood { get; set; }

        [Column("stone")]
        public int MarketStone { get; set; }

        [Column("food")]
        public int MarketFood { get; set; }

        [Column("iron")]
        public int MarketIron { get; set; }

        [Column("gold_ore")]
        public int MarketGoldOre { get; set; }

        [Column("copper")]
        public int MarketCopper { get; set; }

        [Column("gems")]
        public int MarketGems { get; set; }

        [Column("leather")]
        public int MarketLeather { get; set; }

        [Column("player_money")]
        public int MarketMoney { get; set; }
    }
}
