using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandConquest.Entities
{
    public class ChatMessages
    {
        [Required]
        [Column("player_name")]
        public string PlayerName { get; set; }

        [Required]
        [Column("player_messages")]
        public string PlayerMessage { get; set; }

        [Required]
        [Column("message_sent_time")]
        public DateTime MessageTime { get; set; }

    }
}
