using System;

namespace LandConquestDB.Entities
{
    public sealed class Law
    {
        public string LawId { get; set; }
        public int CountryId { get; set; }
        public int Operation { get; set; }
        public string PlayerId { get; set; }
        public string PersonId { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public DateTime InitDateTime { get; set; }

    }
}
