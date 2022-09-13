using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandConquestDB.Entities
{
    public sealed class LawVote
    {
        public string LawId { get; set; }
        public string PersonId { get; set; }
        public string PlayerId { get; set; }
        public int VoteValue { get; set; }
        public DateTime VoteDateTime { get; set; }
    }
}
