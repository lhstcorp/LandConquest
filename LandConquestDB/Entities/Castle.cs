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
    }
}
