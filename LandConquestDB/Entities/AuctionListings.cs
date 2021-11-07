using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LandConquestDB.Entities
{
    public class AuctionListings: INotifyPropertyChanged
    {
        public string ListingId { get; set; }
        public int Qty { get; set; }
        public string ItemName { get; set; }
        public string ItemGroup { get; set; }
        public string ItemSubgroup { get; set; }
        public DateTime ItemSetTime { get; set; }
        public int Price { get; set; } 
        public string SellerName { get; set; }
        public string SellerId { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
