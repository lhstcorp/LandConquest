namespace LandConquestDB.Entities
{
    public sealed class Law
    {
        public int CountryId { get; set; }
        public int Operation { get; set; }
        public string PlayerId { get; set; }
        public string PersonId { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }

    }
}
