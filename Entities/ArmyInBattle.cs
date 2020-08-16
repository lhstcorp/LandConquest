using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandConquest.Entities
{
    public class ArmyInBattle: Army
    {
        [Required]
        [Column("local_land_id")]
        public int LocalLandId { get; set; }

        [Required]
        [Column("army_side")]
        public int ArmySide { get; set; }
    }
}
