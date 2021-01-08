using LandConquest.Entities;
using System;
using System.Data.SqlClient;

namespace LandConquest.Models
{
    public class StorageModel
    {
        public static PlayerStorage GetPlayerStorage(Player player, PlayerStorage storage)
        {
            String storageQuery = "SELECT * FROM dbo.StorageData WHERE player_id = @player_id";

            var command = new SqlCommand(storageQuery, DbContext.GetConnection());
            command.Parameters.AddWithValue("@player_id", player.PlayerId);

            using (var reader = command.ExecuteReader())
            {
                var playerId = reader.GetOrdinal("player_id");
                var playerWood = reader.GetOrdinal("wood");
                var playerStone = reader.GetOrdinal("stone");
                var playerFood = reader.GetOrdinal("food");
                var playerIron = reader.GetOrdinal("iron");
                var playerGoldOre = reader.GetOrdinal("gold_ore");
                var playerCopper = reader.GetOrdinal("copper");
                var playerGems = reader.GetOrdinal("gems");
                var playerLeather = reader.GetOrdinal("leather");
                while (reader.Read())
                {
                    storage.PlayerId = reader.GetString(playerId);
                    storage.PlayerWood = reader.GetInt32(playerWood);
                    storage.PlayerStone = reader.GetInt32(playerStone);
                    storage.PlayerFood = reader.GetInt32(playerFood);
                    storage.PlayerIron = reader.GetInt32(playerIron);
                    storage.PlayerGoldOre = reader.GetInt32(playerGoldOre);
                    storage.PlayerCopper = reader.GetInt32(playerCopper);
                    storage.PlayerGems = reader.GetInt32(playerGems);
                    storage.PlayerLeather = reader.GetInt32(playerLeather);
                }
            }

            command.Dispose();
            return storage;
        }

        public static void UpdateStorage(Player player, PlayerStorage _storage)
        {
            String storageQuery = "UPDATE dbo.StorageData SET wood = @wood, stone  = @stone, food = @food, gold_ore = @gold_ore, copper = @copper, gems = @gems, iron = @iron, leather = @leather WHERE player_id = @player_id ";

            var storageCommand = new SqlCommand(storageQuery, DbContext.GetConnection());
            // int datetimeResult;
            storageCommand.Parameters.AddWithValue("@wood", _storage.PlayerWood);
            storageCommand.Parameters.AddWithValue("@stone", _storage.PlayerStone);
            storageCommand.Parameters.AddWithValue("@food", _storage.PlayerFood);
            storageCommand.Parameters.AddWithValue("@copper", _storage.PlayerCopper);
            storageCommand.Parameters.AddWithValue("@iron", _storage.PlayerIron);
            storageCommand.Parameters.AddWithValue("@gems", _storage.PlayerGems);
            storageCommand.Parameters.AddWithValue("@gold_ore", _storage.PlayerGoldOre);
            storageCommand.Parameters.AddWithValue("@leather", _storage.PlayerLeather);
            storageCommand.Parameters.AddWithValue("@player_id", player.PlayerId);


            for (int i = 0; i < 3; i++)
            {

                storageCommand.Parameters["@wood"].Value = _storage.PlayerWood;
                storageCommand.Parameters["@stone"].Value = _storage.PlayerStone;
                storageCommand.Parameters["@food"].Value = _storage.PlayerFood;
                storageCommand.Parameters["@player_id"].Value = player.PlayerId;
                storageCommand.ExecuteNonQuery();

            }

            storageCommand.Dispose();
        }
    }
}
