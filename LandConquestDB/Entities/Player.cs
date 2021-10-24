namespace LandConquestDB.Entities
{
    using System.Data.Linq.Mapping;
    using System.Drawing;

    [Table(Name = "PlayerData")]
    public class Player
    {
        [Column(IsPrimaryKey = true, Name = "player_id")]
        public string PlayerId { get; set; }
        [Column(Name = "player_name")]
        public string PlayerName { get; set; }
        [Column(Name = "player_exp")]
        public long PlayerExp { get; set; }
        [Column(Name = "player_lvl")]
        public int PlayerLvl { get; set; }
        [Column(Name = "player_money")]
        public long PlayerMoney { get; set; }
        [Column(Name = "player_donation")]
        public long PlayerDonation { get; set; }
        [Column(Name = "player_image")]
        public Image PlayerImage { get; set; }
        [Column(Name = "player_title")]
        public int PlayerTitle { get; set; }
        [Column(Name = "player_current_region")]
        public int PlayerCurrentRegion { get; set; }

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
    }
}
