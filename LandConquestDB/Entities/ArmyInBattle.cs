namespace LandConquestDB.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class ArmyInBattle : Army
    {
        [Required]
        [Column("local_land_id")]
        public int LocalLandId { get; set; }

        [Required]
        [Column("army_side")]
        public int ArmySide { get; set; }

        public bool CanMove { get; set; }

        public bool CanShoot { get; set; }
    }
}
