using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LandConquest.Entities
{
    public class Battle
    {
        [Required]
        [Column("battle_id")]
        [StringLength(16)]
        public string BattleId { get; set; }

        [Required]
        [Column("battle_id")]
        [StringLength(16)]
        public string WarId { get; set; }

        [Column("local_land_id")]
        public int LocalLandId { get; set; }
    }
}
