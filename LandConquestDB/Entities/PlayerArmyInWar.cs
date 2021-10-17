using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandConquestDB.Entities
{
    public class PlayerArmyInWar
    {
        public string PlayerId { get; set; }
        public int ArmyArchersCount { get; set; }
        public int ArmyInfantryCount { get; set; }
        public int ArmyHorsemanCount { get; set; }
        public int ArmySiegegunCount { get; set; }
        public int ArmySide { get; set; }
        public string WarId { get; set; }
    }
}
