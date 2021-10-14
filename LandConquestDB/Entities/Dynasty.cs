using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandConquestDB.Entities
{
    class Dynasty
    {
        public string DynastyId { get; set; }
        public string DynastyName { get; set; }
        public string PlayerId { get; set; }
        public int Prestige { get; set; }
        public int Level { get; set; }
        public Dynasty() { }
    }
}
