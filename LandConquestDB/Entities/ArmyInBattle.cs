namespace LandConquestDB.Entities
{
    public class ArmyInBattle : Army
    {
        public int LocalLandId { get; set; }

        public int ArmySide { get; set; }
        public string WarId { get; set; }
        public bool CanMove { get; set; }

        public bool CanShoot { get; set; }
       

        public ArmyInBattle() { }

        public ArmyInBattle(Army army)
        {
            this.PlayerId = army.PlayerId;
            this.PlayerNameForArmy = army.PlayerNameForArmy;
            this.ArmyId = army.ArmyId;
            this.ArmySizeCurrent = army.ArmySizeCurrent;
            this.ArmyType = army.ArmyType;
            this.ArmyArchersCount = army.ArmyArchersCount;
            this.ArmyInfantryCount = army.ArmyInfantryCount;
            this.ArmyHorsemanCount = army.ArmyHorsemanCount;
            this.ArmySiegegunCount = army.ArmySiegegunCount;
        }

        public void calculateSizeCurrent()
        {
            this.ArmySizeCurrent = this.ArmyInfantryCount + this.ArmyArchersCount + this.ArmyHorsemanCount + this.ArmySiegegunCount;
        }
    }
}
