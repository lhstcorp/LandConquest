using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandConquest.Entities
{
    public class Auction
    {
        [Required]
        [Column("qty")]
        public int Qty { get; set; }

        [Required]
        [Column("item_group")]
        public string SubjectGroup { get; set; }

        [Required]
        [Column("item_subgroup")]
        public string SubjectSubgroup { get; set; }

        [Required]
        [Column("item")]
        public string Subject { get; set; }

        [Required]
        [Column("listing_set_time")]
        public DateTime ListingSetTime { get; set; }

        [Required]
        [Column("seller_name")]
        public string SellerName { get; set; }

        [Required]
        [Column("price")]
        public int Price { get; set; }

        [Required]
        [Column("seller_id")]
        public string SellerId { get; set; }
    }
}
