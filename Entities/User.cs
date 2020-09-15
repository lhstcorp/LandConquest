namespace LandConquest.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class User
    {
        [Required]
        [Column("user_id")]
        [StringLength(16)]
        public string UserId { get; set; }

        [Required]
        [Column("user_login")]
        [StringLength(20)]
        public string UserLogin { get; set; }

        [Required]
        [Column("user_email")]
        [StringLength(50)]
        public string UserEmail { get; set; }

        [Required]
        [Column("user_pass")]
        [StringLength(20)]
        public string UserPass { get; set; }

    }
}
