using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandConquest.Entities
{
    public class PlayerEquipment
    {
        [Required]
        [Column("player_id")]
        [StringLength(16)]
        public string PlayerId { get; set; }

        [Column("armor")]
        public int PlayerArmor { get; set; }

        [Column("sword")]
        public int PlayerSword { get; set; }

        [Column("harness")]
        public int PlayerHarness { get; set; }

        [Column("spear")]
        public int PlayerSpear { get; set; }

        [Column("bow")]
        public int PlayerBow { get; set; }

        [Column("gear")]
        public int PlayerGear { get; set; }

        
    }
}
