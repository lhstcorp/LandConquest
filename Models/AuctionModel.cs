using LandConquest.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LandConquest.Models
{
    public class AuctionModel
    {
        public void AddListing(int qty, string itemName, string itemGroup, string itemSubgroup, int price, Player player, SqlConnection connection)
        {
            String addListingQuery = "INSERT INTO dbo.AuctionListings (listing_id,qty,item_name,item_group,item_subgroup,item_set_time,price,seller_name,seller_id) VALUES (@listing_id,@qty,@item_name,@item_group,@item_subgroup,@item_set_time,@price,@seller_name,@seller_id)";
            var auctionCommand = new SqlCommand(addListingQuery, connection);

            auctionCommand.Parameters.AddWithValue("@listing_id", generateListingId());
            auctionCommand.Parameters.AddWithValue("@qty", qty);
            auctionCommand.Parameters.AddWithValue("@item_name", itemName);
            auctionCommand.Parameters.AddWithValue("@item_group", itemGroup);
            auctionCommand.Parameters.AddWithValue("@item_subgroup", itemSubgroup);
            auctionCommand.Parameters.AddWithValue("@item_set_time", DateTime.UtcNow);
            auctionCommand.Parameters.AddWithValue("@price", price);
            auctionCommand.Parameters.AddWithValue("@seller_name", player.PlayerName);
            auctionCommand.Parameters.AddWithValue("@seller_id", player.PlayerId);
            auctionCommand.ExecuteNonQuery();
            auctionCommand.Dispose();

            String storageQuery = "UPDATE dbo.StorageData SET " + itemName + " = " + itemName + " - @item_amount WHERE player_id = @player_id ";  //нужна доп валидация
            var storageCommand = new SqlCommand(storageQuery, connection);
            storageCommand.Parameters.AddWithValue("@item_name", itemName);
            storageCommand.Parameters.AddWithValue("@item_amount", qty);
            storageCommand.Parameters.AddWithValue("@player_id", player.PlayerId);
            storageCommand.ExecuteNonQuery();
            storageCommand.Dispose();

        }

        public void BuyListing(int itemQty, int moneyAmount, string itemName, string itemGroup, string itemSubgroup, int price, Player playerCustomer, Player playerSeller, AuctionListings listing, SqlConnection connection)
        {
            String listingQuery = "UPDATE dbo.AuctionData SET qty = qty - @qty WHERE listing_id = @listing_id ";  //нужна доп валидация
            var listingCommand = new SqlCommand(listingQuery, connection);
            listingCommand.Parameters.AddWithValue("@qty", itemQty);
            listingCommand.Parameters.AddWithValue("@listing_id", listing.ListingId);
            listingCommand.ExecuteNonQuery();
            listingCommand.Dispose();

            String storageQuery = "UPDATE dbo.StorageData SET " + itemName + " = " + itemName + " + @item_amount WHERE player_id = @player_id ";  //нужна доп валидация
            var storageCommand = new SqlCommand(storageQuery, connection);
            storageCommand.Parameters.AddWithValue("@item_name", itemName);
            storageCommand.Parameters.AddWithValue("@item_amount", itemQty);
            storageCommand.Parameters.AddWithValue("@player_id", playerCustomer.PlayerId);
            storageCommand.ExecuteNonQuery();
            storageCommand.Dispose();

            playerCustomer.PlayerMoney = playerCustomer.PlayerMoney - moneyAmount;
            playerSeller.PlayerMoney = playerSeller.PlayerMoney + moneyAmount;

            PlayerModel model = new PlayerModel();
            model.UpdatePlayerMoney(playerCustomer, connection);
            model.UpdatePlayerMoney(playerSeller, connection);
        }

        public List<AuctionListings> GetListings(List<AuctionListings> listings, SqlConnection connection)
        {
            String query = "SELECT * FROM dbo.AuctionListings";
            List<string> ListingId = new List<string>();
            List<int> Qty = new List<int>();
            List<string> ItemName = new List<string>();
            List<string> ItemGroup = new List<string>();
            List<string> ItemSubgroup = new List<string>();
            List<DateTime> ItemSetTime = new List<DateTime>();
            List<int> Price = new List<int>();
            List<string> SellerName = new List<string>();
            List<string> SellerId = new List<string>();


            var command = new SqlCommand(query, connection);

            using (var reader = command.ExecuteReader())
            {
                var listingId = reader.GetOrdinal("listing_id");
                var qty = reader.GetOrdinal("qty");
                var itemName = reader.GetOrdinal("item_name");
                var itemGroup = reader.GetOrdinal("item_group");
                var itemSubgroup = reader.GetOrdinal("item_subgroup");
                var itemSetTime = reader.GetOrdinal("item_set_time");
                var price = reader.GetOrdinal("price");
                var sellerName = reader.GetOrdinal("seller_name");
                var sellerId = reader.GetOrdinal("seller_id");


                while (reader.Read())
                {
                    ListingId.Add(reader.GetString(listingId));
                    Qty.Add(reader.GetInt32(qty));
                    ItemName.Add(reader.GetString(itemName));
                    ItemGroup.Add(reader.GetString(itemGroup));
                    ItemSubgroup.Add(reader.GetString(itemSubgroup));
                    ItemSetTime.Add(reader.GetDateTime(itemSetTime));
                    Price.Add(reader.GetInt32(price));
                    SellerName.Add(reader.GetString(sellerName));
                    SellerId.Add(reader.GetString(sellerId));
                }
            }
            command.Dispose();

            listings = new List<AuctionListings>();
            for (int i = 0; i < ListingId.Count; i++)
            {
                listings.Add(new AuctionListings());
            }

            for (int i = 0; i < listings.Count; i++)
            {
                listings[i].ListingId = ListingId[i];
                listings[i].Qty = Qty[i];
                listings[i].Subject = ItemName[i];
                listings[i].SubjectGroup = ItemGroup[i];
                listings[i].SubjectSubgroup = ItemSubgroup[i];
                listings[i].ListingSetTime = ItemSetTime[i];
                listings[i].Price = Price[i];
                listings[i].SellerName = SellerName[i];
                listings[i].SellerId = SellerId[i];
            }
            return listings;
        }

        public List<AuctionListings> FindListings(List<AuctionListings> listings, SqlConnection connection)
        {
            String query = "SELECT * FROM dbo.AuctionListings where item_name = item_name";
            List<string> ListingId = new List<string>();
            List<int> Qty = new List<int>();
            List<string> ItemName = new List<string>();
            List<string> ItemGroup = new List<string>();
            List<string> ItemSubgroup = new List<string>();
            List<DateTime> ItemSetTime = new List<DateTime>();
            List<int> Price = new List<int>();
            List<string> SellerName = new List<string>();
            List<string> SellerId = new List<string>();


            var command = new SqlCommand(query, connection);

            using (var reader = command.ExecuteReader())
            {
                var listingId = reader.GetOrdinal("listing_id");
                var qty = reader.GetOrdinal("qty");
                var itemName = reader.GetOrdinal("item_name");
                var itemGroup = reader.GetOrdinal("item_group");
                var itemSubgroup = reader.GetOrdinal("item_subgroup");
                var itemSetTime = reader.GetOrdinal("item_set_time");
                var price = reader.GetOrdinal("price");
                var sellerName = reader.GetOrdinal("seller_name");
                var sellerId = reader.GetOrdinal("seller_id");


                while (reader.Read())
                {
                    ListingId.Add(reader.GetString(listingId));
                    Qty.Add(reader.GetInt32(qty));
                    ItemName.Add(reader.GetString(itemName));
                    ItemGroup.Add(reader.GetString(itemGroup));
                    ItemSubgroup.Add(reader.GetString(itemSubgroup));
                    ItemSetTime.Add(reader.GetDateTime(itemSetTime));
                    Price.Add(reader.GetInt32(price));
                    SellerName.Add(reader.GetString(sellerName));
                    SellerId.Add(reader.GetString(sellerId));
                }
            }
            command.Dispose();

            listings = new List<AuctionListings>();
            for (int i = 0; i < ListingId.Count; i++)
            {
                listings.Add(new AuctionListings());
            }

            for (int i = 0; i < listings.Count; i++)
            {
                listings[i].ListingId = ListingId[i];
                listings[i].Qty = Qty[i];
                listings[i].Subject = ItemName[i];
                listings[i].SubjectGroup = ItemGroup[i];
                listings[i].SubjectSubgroup = ItemSubgroup[i];
                listings[i].ListingSetTime = ItemSetTime[i];
                listings[i].Price = Price[i];
                listings[i].SellerName = SellerName[i];
                listings[i].SellerId = SellerId[i];
            }
            return listings;
        }



        private static Random random;
        public static string generateListingId()
        {
            Thread.Sleep(15);
            random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvmxyz0123456789";
            return new string(Enumerable.Repeat(chars, 16)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
