namespace LandConquestDB.Entities
{
    public class DistanceBattle
    {
        public string BattleId { get; set; }
        public string WarId { get; set; }
        public int LocalLandId { get; set; }
        public int Damage { get; set; }
        public int Side { get; set; }
    }
}
