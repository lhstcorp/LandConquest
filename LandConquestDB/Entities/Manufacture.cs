namespace LandConquestDB.Entities
{
    using System;
    public class Manufacture
    {
        public string PlayerId { get; set; }
        public string ManufactureId { get; set; }
        public string ManufactureName { get; set; }
        public int ManufactureType { get; set; }
        public int ManufactureLvl { get; set; }
        public int ManufacturePeasantMax { get; set; }
        public int ManufacturePeasantWork { get; set; }
        public int ManufactureProductsHour { get; set; }
        public DateTime ManufactureProdStartTime { get; set; }
        public int ManufactureBaseProdValue { get; set; }
        public int WarehouseId { get; set; }



    }
}
