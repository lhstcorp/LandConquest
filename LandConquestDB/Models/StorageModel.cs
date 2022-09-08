using LandConquestDB.Entities;
using System.Data.SqlClient;

namespace LandConquestDB.Models
{
    public class StorageModel
    {
        public static PlayerStorage GetPlayerStorage(Player player)
        {
            PlayerStorage storage = new PlayerStorage();
            string storageQuery = "SELECT * FROM dbo.StorageData WHERE player_id = @player_id";

            var command = new SqlCommand(storageQuery, DbContext.GetSqlConnection());
            command.Parameters.AddWithValue("@player_id", player.PlayerId);

            using (var reader = command.ExecuteReader())
            {
                var playerId = reader.GetOrdinal("player_id");
                var playerWood = reader.GetOrdinal("wood");
                var playerStone = reader.GetOrdinal("clay");
                var playerFood = reader.GetOrdinal("coal");
                var playerIron = reader.GetOrdinal("fur");
                var playerGoldOre = reader.GetOrdinal("wool");
                var playerCopper = reader.GetOrdinal("soda");
                var playerGems = reader.GetOrdinal("lime");
                var playerLeather = reader.GetOrdinal("leather");
                while (reader.Read())
                {
                    storage.PlayerId = reader.GetString(playerId);
                    storage.Wood = reader.GetInt32(playerWood);
                    storage.Stone = reader.GetInt32(playerStone);
                    storage.Food = reader.GetInt32(playerFood);
                    storage.Iron = reader.GetInt32(playerIron);
                    storage.GoldOre = reader.GetInt32(playerGoldOre);
                    storage.Copper = reader.GetInt32(playerCopper);
                    storage.Gems = reader.GetInt32(playerGems);
                    storage.Leather = reader.GetInt32(playerLeather);
                }
                reader.Close();
            }
            command.Dispose();
            return storage;
        }

        public static void UpdateStorage(Player player, PlayerStorage _storage)
        {
            string storageQuery = "UPDATE dbo.StorageData SET wood = @wood, stone  = @stone, food = @food, gold_ore = @gold_ore, copper = @copper, gems = @gems, iron = @iron, leather = @leather WHERE player_id = @player_id ";

            var storageCommand = new SqlCommand(storageQuery, DbContext.GetSqlConnection());
            // int datetimeResult;
            storageCommand.Parameters.AddWithValue("@wood", _storage.Wood);
            storageCommand.Parameters.AddWithValue("@stone", _storage.Stone);
            storageCommand.Parameters.AddWithValue("@food", _storage.Food);
            storageCommand.Parameters.AddWithValue("@copper", _storage.Copper);
            storageCommand.Parameters.AddWithValue("@iron", _storage.Iron);
            storageCommand.Parameters.AddWithValue("@gems", _storage.Gems);
            storageCommand.Parameters.AddWithValue("@gold_ore", _storage.GoldOre);
            storageCommand.Parameters.AddWithValue("@leather", _storage.Leather);
            storageCommand.Parameters.AddWithValue("@player_id", player.PlayerId);


            for (int i = 0; i < 3; i++)
            {

                storageCommand.Parameters["@wood"].Value = _storage.Wood;
                storageCommand.Parameters["@stone"].Value = _storage.Stone;
                storageCommand.Parameters["@food"].Value = _storage.Food;
                storageCommand.Parameters["@player_id"].Value = player.PlayerId;
                storageCommand.ExecuteNonQuery();
            }

            storageCommand.Dispose();
        }
    }
}
