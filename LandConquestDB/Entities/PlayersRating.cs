using System;

namespace LandConquestDB.Entities
{
    public class PlayersRating
    {
        public string PlayerId { get; set; }
        public string Name { get; set; }
        public string Qty { get; set; }
        public PlayersRating(string playerId, string name, string qty)
        {
            PlayerId = playerId;
            Name = name;
            Qty = qty;
        }

      


    }
}
