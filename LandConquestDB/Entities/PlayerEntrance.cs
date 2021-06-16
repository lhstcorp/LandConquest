namespace LandConquestDB.Entities
{
    using System;
    public class PlayerEntrance
    {
        public string PlayerId { get; set; }
        public string PlayerNameForEntrance { get; set; }
        public DateTime LastEntrance { get; set; }
        public DateTime FirstEntrance { get; set; }
    }
}
