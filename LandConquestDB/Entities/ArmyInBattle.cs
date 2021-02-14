namespace LandConquestDB.Entities
{
    public class ArmyInBattle : Army
    {
        public int LocalLandId { get; set; }

        public int ArmySide { get; set; }

        public bool CanMove { get; set; }

        public bool CanShoot { get; set; }
    }
}
