using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace LandConquestDB.Entities
{
    public class GarrisonListings
    {
        public string ArmyId { get; set; }
        public string PlayerName { get; set; }
        public BitmapImage ArmyTypeImg { get; set; }
        public int SlotId { get; set; }
        public int Inf { get; set; }
        public int Ar { get; set; }
        public int Kn { get; set; }
        public int Sie { get; set; }
        public int Total { get; set; }
    }
}
