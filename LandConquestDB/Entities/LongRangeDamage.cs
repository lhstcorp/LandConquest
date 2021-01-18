using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandConquestDB.Entities
{
    class LongRangeDamage
    {
        [Required]
        [Column("army_id")]
        [StringLength(16)]
        public string ArmyId { get; set; }


        [Required]
        [Column("damage")]
        public int Damage { get; set; }
    }
}
