using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandConquest.Entities
{
    public class Peasants
    {
        [Required]
        [Column("player_id")]
        [StringLength(16)]
        public string PlayerId { get; set; }

        [Column("peasants_count")]
        public int PeasantsCount { get; set; }

        [Column("peasants_work")]
        public int PeasantsWork { get; set; }

        [Column("peasants_max")]
        public int PeasantsMax { get; set; }
    }
}
