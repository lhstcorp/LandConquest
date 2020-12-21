using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandConquest.Entities
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
