using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandConquestDB.Entities
{
    public class LogWar
    {
        public string LogId { get; set; }
        public string WarId { get; set; }
        public int LocalLandId { get; set; }
        public int AttackersLost { get; set; }
        public int DefendersLost { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
