using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandConquestDB.Enums
{
    public class ItemTypeEnum
    {
        public enum ItemType : int
        {
            Agriculture = 0,
            Metal = 1,
            RawMaterials = 2,
            Tools = 3,
            Armament = 4,
            Clothing = 5,
            Products = 6,
            ConstructionMaterials = 7,
        }
    }
}
