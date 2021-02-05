namespace LandConquestDB.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class PlayerEntrance
    {
        [Required]
        [Column("player_id")]
        [StringLength(16)]
        public string PlayerId { get; set; }

        [Required]
        [Column("player_name")]
        [StringLength(20)]
        public string PlayerNameForEntrance { get; set; }

        [Column("last_entrance")]
        public DateTime LastEntrance { get; set; }

        [Column("first_entrance")]
        public DateTime FirstEntrance { get; set; }
    }
}
