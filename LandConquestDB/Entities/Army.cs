namespace LandConquestDB.Entities
{
    public class Army
    {
        public string PlayerId { get; set; }
        public string ArmyId { get; set; }
        public int ArmySizeCurrent { get; set; }
        public int ArmyType { get; set; }
        public int ArmyArchersCount { get; set; }
        public int ArmyInfantryCount { get; set; }

        public int ArmyHorsemanCount { get; set; }

        public int ArmySiegegunCount { get; set; }
        public string PlayerNameForArmy { get; set; }
    }
}
