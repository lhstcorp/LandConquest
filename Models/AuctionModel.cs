using LandConquest.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandConquest.Models
{
    public class AuctionModel
    {
        public void AddListing(int qty, string itemName, string itemGroup, string itemSubgroup, int price, Player player, SqlConnection connection)
        {
            String addListingQuery = "INSERT INTO dbo.AuctionListings (qty,item_name,item_group,item_subgroup,item_set_time,price,seller_name,seller_id) VALUES (@qty,@item_name,@item_group,@item_subgroup,@item_set_time,@price,@seller_name,@seller_id)";
            var auctionCommand = new SqlCommand(addListingQuery, connection);

            auctionCommand.Parameters.AddWithValue("@qty", qty);
            auctionCommand.Parameters.AddWithValue("@item_name", itemName);
            auctionCommand.Parameters.AddWithValue("@item_group", itemGroup);
            auctionCommand.Parameters.AddWithValue("@item_subgroup", itemSubgroup);
            auctionCommand.Parameters.AddWithValue("@item_set_time", System.DateTime.UtcNow);
            auctionCommand.Parameters.AddWithValue("@price", price);
            auctionCommand.Parameters.AddWithValue("@seller_name", player.PlayerName);
            auctionCommand.Parameters.AddWithValue("@seller_id", player.PlayerId);

            auctionCommand.ExecuteNonQuery();

            auctionCommand.Dispose();
        }
    }
}
