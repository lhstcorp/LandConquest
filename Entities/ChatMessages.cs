namespace LandConquest.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

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
