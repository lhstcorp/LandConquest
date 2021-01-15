namespace LandConquestDB.Entities
{
    public class PlayersRating
    {
        public PlayersRating(string playerId, string name, int qty)
        {
            PlayerId = playerId;
            Name = name;
            Qty = qty;
        }
        public string PlayerId { get; set; }
        public string Name { get; set; }
        public int Qty { get; set; }


    }
}
