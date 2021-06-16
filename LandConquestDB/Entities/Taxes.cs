namespace LandConquestDB.Entities
{
    using System;
    public class Taxes
    {
        public string PlayerId { get; set; }
        public int TaxValue { get; set; }
        public int TaxMoneyHour { get; set; }
        public DateTime TaxSaveDateTime { get; set; }
    }
}
