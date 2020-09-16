namespace LandConquest.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class Manufacture
    {
        [Required]
        [Column("player_id")]
        [StringLength(16)]
        public string PlayerId { get; set; }

        [Required]
        [Column("manufacture_id")]
        [StringLength(16)]
        public string ManufactureId { get; set; }

        [Required]
        [Column("manufacture_name")]
        [StringLength(22)]
        public string ManufactureName { get; set; }

        [Column("manufacture_type")]
        public int ManufactureType { get; set; }

        [Column("manufacture_lvl")]
        public int ManufactureLevel { get; set; }

        [Column("manufacture_peasant_max")]
        public int ManufacturePeasantMax { get; set; }

        [Column("manufacture_peasant_work")]
        public int ManufacturePeasantWork { get; set; }

        [Column("manufacture_products_hour")]
        public int ManufactureProductsHour { get; set; }

        [Column("manufacture_prod_start_time")]
        public DateTime ManufactureProdStartTime { get; set; }

        [Column("manufacture_base_prod_value")]
        public int ManufactureBaseProdValue { get; set; }



    }
}
