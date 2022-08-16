using Dapper;
using LandConquestDB.Entities;
using System.Data.SqlClient;
using System.Linq;

namespace LandConquestDB.Models
{
    public class MarketModel
    {
        public static Market GetMarketInfo(Player player, Market market)
        {
            string marketQuery = "SELECT * FROM dbo.MarketData";

            //return DbContext.GetSqlConnection().Query<Market>("SELECT * FROM dbo.MarketData").FirstOrDefault();

            var command = new SqlCommand(marketQuery, DbContext.GetSqlConnection());
            command.Parameters.AddWithValue("@player_id", player.PlayerId);

            using (var reader = command.ExecuteReader())
            {
                var playerId = reader.GetOrdinal("player_id");
                var marketWood = reader.GetOrdinal("wood");
                var marketStone = reader.GetOrdinal("stone");
                var marketFood = reader.GetOrdinal("food");
                var marketIron = reader.GetOrdinal("iron");
                var marketGoldOre = reader.GetOrdinal("gold_ore");
                var marketCopper = reader.GetOrdinal("copper");
                var marketGems = reader.GetOrdinal("gems");
                var marketLeather = reader.GetOrdinal("leather");
                var money = reader.GetOrdinal("money");
                while (reader.Read())
                {
                    market.PlayerId = reader.GetString(playerId);
                    market.Wood = reader.GetInt32(marketWood);
                    market.Stone = reader.GetInt32(marketStone);
                    market.Food = reader.GetInt32(marketFood);
                    market.Iron = reader.GetInt32(marketIron);
                    market.GoldOre = reader.GetInt32(marketGoldOre);
                    market.Copper = reader.GetInt32(marketCopper);
                    market.Gems = reader.GetInt32(marketGems);
                    market.Leather = reader.GetInt32(marketLeather);
                    //market.MarketMoney = reader.GetInt32(money);
                }
                reader.Close();
            }
            command.Dispose();
            return market;
        }
        public static void UpdateMarket(Player player, Market _market)
        {
            string marketQuery = "UPDATE dbo.MarketData SET wood = @wood, stone  = @stone, food = @food, gold_ore = @gold_ore, copper = @copper, gems = @gems, iron = @iron, leather = @leather, money = @money";

            //DbContext.GetSqlConnection().Execute("UPDATE dbo.MarketData SET wood = @wood, stone  = @stone, food = @food, gold_ore = @gold_ore, copper = @copper, gems = @gems, iron = @iron, leather = @leather, money = @money", new { wood = _market.Wood, stone = _market.Stone, food = _market.Food, gold_ore = _market.GoldOre, copper = _market.Copper, iron = _market.Iron, leather = _market.Leather, money = _market.Money});


            var marketCommand = new SqlCommand(marketQuery, DbContext.GetSqlConnection());
            // int datetimeResult;
            marketCommand.Parameters.AddWithValue("@wood", _market.Wood);
            marketCommand.Parameters.AddWithValue("@stone", _market.Stone);
            marketCommand.Parameters.AddWithValue("@food", _market.Food);
            marketCommand.Parameters.AddWithValue("@copper", _market.Copper);
            marketCommand.Parameters.AddWithValue("@iron", _market.Iron);
            marketCommand.Parameters.AddWithValue("@gems", _market.Gems);
            marketCommand.Parameters.AddWithValue("@gold_ore", _market.GoldOre);
            marketCommand.Parameters.AddWithValue("@leather", _market.Leather);
            marketCommand.Parameters.AddWithValue("@money", _market.Money);
            marketCommand.Parameters.AddWithValue("@player_id", player.PlayerId);


            for (int i = 0; i < 9; i++)
            {

                marketCommand.Parameters["@wood"].Value = _market.Wood;
                marketCommand.Parameters["@stone"].Value = _market.Stone;
                marketCommand.Parameters["@food"].Value = _market.Food;
                marketCommand.Parameters["@copper"].Value = _market.Copper;
                marketCommand.Parameters["@iron"].Value = _market.Iron;
                marketCommand.Parameters["@gems"].Value = _market.Gems;
                marketCommand.Parameters["@gold_ore"].Value = _market.GoldOre;
                marketCommand.Parameters["@leather"].Value = _market.Leather;
                marketCommand.Parameters["@money"].Value = _market.Money;
                marketCommand.Parameters["@player_id"].Value = player.PlayerId;
                marketCommand.ExecuteNonQuery();

            }

            marketCommand.Dispose();
        }
    }
}
