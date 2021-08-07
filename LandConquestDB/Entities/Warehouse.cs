using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandConquestDB.Entities
{
    public class Warehouse
    {
        public int Id { get; set; }
        public int LandId { get; set; }
        public string PlayerId { get; set; }

        public string LandName { get; set; } //не уверен что необходимо, для удобства пусть будет
    }
}
