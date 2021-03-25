using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandConquestDB.Entities
{
    public sealed class Castle
    {
        public int LandId { get; set; }
        public int CastleLvl { get; set; }
        public int SlotsCount { get; set; }

        public int returnMaxTroopsInSlot()
        {
            int maxTroops = 0;

            if (this.CastleLvl >= 800)
            {
                maxTroops = 1000 + (this.CastleLvl - 800) * 20;
            }
            else
            {
                maxTroops = 1000;
            }

            return maxTroops;
        }

        public int returnMaxTroops()
        {
            return returnMaxTroopsInSlot() * 9;
        }
    }
}
