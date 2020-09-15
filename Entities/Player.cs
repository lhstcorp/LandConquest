namespace LandConquest.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Windows.Controls;
    public class Player
    {
        public Player(string _playerId, string _playerName)
        {
            PlayerId = _playerId;
            PlayerName = _playerName;
            PlayerLvl = 1;
            PlayerCurrentRegion = 1;
            PlayerImage = null;
            PlayerExp = 0;
            PlayerMoney = 1;
            PlayerDonation = 2;
            PlayerTitle = 1;
        }

        public Player()
        { }

        [Required]
        [Column("player_id")]
        [StringLength(16)]
        public string PlayerId { get; set; }

        [Required]
        [Column("player_name")]
        [StringLength(20)]
        public string PlayerName { get; set; }

        [Column("player_exp")]
        public long PlayerExp { get; set; }

        [Required]
        [Column("player_lvl")]
        public int PlayerLvl { get; set; }

        [Column("player_money")]
        public long PlayerMoney { get; set; }

        [Column("player_donation")]
        public long PlayerDonation { get; set; }

        [Column("player_image")]
        public Image PlayerImage { get; set; }

        [Required]
        [Column("player_title")]
        public int PlayerTitle { get; set; }

        [Required]
        [Column("player_current_region")]
        public int PlayerCurrentRegion { get; set; }
    }
}
