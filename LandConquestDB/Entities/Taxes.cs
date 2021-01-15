namespace LandConquestDB.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class Taxes
    {
        [Required]
        [Column("player_id")]
        [StringLength(16)]
        public string PlayerId { get; set; }

        [Required]
        [Column("tax_value")]
        public int TaxValue { get; set; }

        [Column("tax_money_hour")]
        public int TaxMoneyHour { get; set; }

        [Column("tax_save_datetime")]
        public DateTime TaxSaveDateTime { get; set; }
    }
}
