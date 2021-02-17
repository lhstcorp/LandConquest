namespace LandConquestDB.Entities
{
    public sealed class Land
    {
        public int LandId { get; set; }
        public string LandName { get; set; }
        public string LandColor { get; set; }
        public int CountryId { get; set; }
        public int ResourceType1 { get; set; }
        public int ResourceType2 { get; set; }
    }
}
