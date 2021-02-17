namespace LandConquestDB.Entities
{
    using System;
    public class War
    {
        public string WarId { get; set; }
        public int LandAttackerId { get; set; }
        public int LandDefenderId { get; set; }
        public DateTime WarDateTimeStart { get; set; }
    }
}
