using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandConquest.Entities
{
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
