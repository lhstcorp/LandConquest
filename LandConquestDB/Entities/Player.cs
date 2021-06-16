namespace LandConquestDB.Entities
{
    using System.Drawing;
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

        public string PlayerId { get; set; }
        public string PlayerName { get; set; }
        public long PlayerExp { get; set; }
        public int PlayerLvl { get; set; }
        public long PlayerMoney { get; set; }
        public long PlayerDonation { get; set; }
        public Image PlayerImage { get; set; }
        public int PlayerTitle { get; set; }
        public int PlayerCurrentRegion { get; set; }
    }
}
