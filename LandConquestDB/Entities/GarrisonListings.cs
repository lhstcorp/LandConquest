using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandConquestDB.Entities
{
    public class GarrisonListings
    {
        public string ArmyId { get; set; }
        public Image ArmyTypeImg { get; set; }
        public int Inf { get; set; }

        public int Ar { get; set; }
        public int Kn { get; set; }
        public int Sie { get; set; }
        public int Total { get; set; }
    }
}
