using LandConquestDB.Entities;
using System.Data.SqlClient;

namespace LandConquestDB.Models
{
    public class MarketModel
    {
        public static Market GetMarketInfo(Player player, Market market)
        {
            string marketQuery = "SELECT * FROM dbo.MarketData";

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
                    market.MarketWood = reader.GetInt32(marketWood);
                    market.MarketStone = reader.GetInt32(marketStone);
                    market.MarketFood = reader.GetInt32(marketFood);
                    market.MarketIron = reader.GetInt32(marketIron);
                    market.MarketGoldOre = reader.GetInt32(marketGoldOre);
                    market.MarketCopper = reader.GetInt32(marketCopper);
                    market.MarketGems = reader.GetInt32(marketGems);
                    market.MarketLeather = reader.GetInt32(marketLeather);
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

            var marketCommand = new SqlCommand(marketQuery, DbContext.GetSqlConnection());
            // int datetimeResult;
            marketCommand.Parameters.AddWithValue("@wood", _market.MarketWood);
            marketCommand.Parameters.AddWithValue("@stone", _market.MarketStone);
            marketCommand.Parameters.AddWithValue("@food", _market.MarketFood);
            marketCommand.Parameters.AddWithValue("@copper", _market.MarketCopper);
            marketCommand.Parameters.AddWithValue("@iron", _market.MarketIron);
            marketCommand.Parameters.AddWithValue("@gems", _market.MarketGems);
            marketCommand.Parameters.AddWithValue("@gold_ore", _market.MarketGoldOre);
            marketCommand.Parameters.AddWithValue("@leather", _market.MarketLeather);
            marketCommand.Parameters.AddWithValue("@money", _market.MarketMoney);
            marketCommand.Parameters.AddWithValue("@player_id", player.PlayerId);


            for (int i = 0; i < 9; i++)
            {

                marketCommand.Parameters["@wood"].Value = _market.MarketWood;
                marketCommand.Parameters["@stone"].Value = _market.MarketStone;
                marketCommand.Parameters["@food"].Value = _market.MarketFood;
                marketCommand.Parameters["@copper"].Value = _market.MarketCopper;
                marketCommand.Parameters["@iron"].Value = _market.MarketIron;
                marketCommand.Parameters["@gems"].Value = _market.MarketGems;
                marketCommand.Parameters["@gold_ore"].Value = _market.MarketGoldOre;
                marketCommand.Parameters["@leather"].Value = _market.MarketLeather;
                marketCommand.Parameters["@money"].Value = _market.MarketMoney;
                marketCommand.Parameters["@player_id"].Value = player.PlayerId;
                marketCommand.ExecuteNonQuery();

            }

            marketCommand.Dispose();
        }
    }
}
