using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandConquestDB.Entities
{
    public class Garrison : ArmyInBattle
    {
        [Required]
        [Column("land_id")]
        public int LandId { get; set; }
    }
}
