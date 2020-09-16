namespace LandConquest.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public sealed class Land
    {
        [Required]
        [Column("land_id")]
        public int LandId { get; set; }

        [Required]
        [Column("land_name")]
        [StringLength(22)]
        public string LandName { get; set; }

        [Required]
        [Column("land_color")]
        [StringLength(9)]
        public string LandColor { get; set; }

        [Column("country_id")]
        public int CountryId { get; set; }

        [Column("resource_type_1")]
        public int ResourceType1 { get; set; }

        [Column("resource_type_2")]
        public int ResourceType2 { get; set; }
    }
}
