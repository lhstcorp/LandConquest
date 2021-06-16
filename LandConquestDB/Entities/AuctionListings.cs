using System;
namespace LandConquestDB.Entities
{
    public class AuctionListings
    {
        public string ListingId { get; set; }
        public int Qty { get; set; }

        public string SubjectGroup { get; set; }
        public string SubjectSubgroup { get; set; }
        public string Subject { get; set; }
        public DateTime ListingSetTime { get; set; }
        public string SellerName { get; set; }
        public int Price { get; set; }
        public string SellerId { get; set; }
    }
}
