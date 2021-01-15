namespace LandConquestDB.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class War
    {
        [Required]
        [Column("war_id")]
        [StringLength(16)]
        public string WarId { get; set; }

        [Required]
        [Column("land_attacker_id")]
        public int LandAttackerId { get; set; }

        [Required]
        [Column("land_defender_id")]
        public int LandDefenderId { get; set; }

        [Column("datetime_start")]
        public DateTime WarDateTimeStart { get; set; }
    }
}
