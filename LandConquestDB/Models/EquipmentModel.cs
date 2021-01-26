using LandConquestDB.Entities;
using System.Data.SqlClient;

namespace LandConquestDB.Models
{
    public class EquipmentModel
    {
        public static PlayerEquipment GetPlayerEquipment(Player player, PlayerEquipment equipment)
        {
            string storageQuery = "SELECT * FROM dbo.PlayerEquipment WHERE player_id = @player_id";

            var command = new SqlCommand(storageQuery, DbContext.GetSqlConnection());
            command.Parameters.AddWithValue("@player_id", player.PlayerId);

            using (var reader = command.ExecuteReader())
            {
                var playerId = reader.GetOrdinal("player_id");
                var playerWood = reader.GetOrdinal("armor");
                var playerStone = reader.GetOrdinal("sword");
                var playerFood = reader.GetOrdinal("harness");
                var playerIron = reader.GetOrdinal("spear");
                var playerGoldOre = reader.GetOrdinal("bow");
                var playerCopper = reader.GetOrdinal("gear");

                while (reader.Read())
                {
                    equipment.PlayerId = reader.GetString(playerId);
                    equipment.PlayerArmor = reader.GetInt32(playerWood);
                    equipment.PlayerSword = reader.GetInt32(playerStone);
                    equipment.PlayerHarness = reader.GetInt32(playerFood);
                    equipment.PlayerSpear = reader.GetInt32(playerIron);
                    equipment.PlayerBow = reader.GetInt32(playerGoldOre);
                    equipment.PlayerGear = reader.GetInt32(playerCopper);

                }
            }
            return equipment;
        }

        public static void UpdateEquipment(Player player, PlayerEquipment _equipment)
        {
            string storageQuery = "UPDATE dbo.PlayerEquipment SET armor = @armor, sword  = @sword, harness = @harness, spear  = @spear, bow = @bow, gear = @gear WHERE player_id = @player_id ";

            var storageCommand = new SqlCommand(storageQuery, DbContext.GetSqlConnection());
            // int datetimeResult;
            storageCommand.Parameters.AddWithValue("@armor", _equipment.PlayerArmor);
            storageCommand.Parameters.AddWithValue("@sword", _equipment.PlayerSword);
            storageCommand.Parameters.AddWithValue("@harness", _equipment.PlayerHarness);
            storageCommand.Parameters.AddWithValue("@spear", _equipment.PlayerSpear);
            storageCommand.Parameters.AddWithValue("@bow", _equipment.PlayerBow);
            storageCommand.Parameters.AddWithValue("@gear", _equipment.PlayerGear);
            storageCommand.Parameters.AddWithValue("@player_id", player.PlayerId);



            //storageCommand.Parameters["@armor"].Value = _equipment.PlayerArmor;
            //storageCommand.Parameters["@sword"].Value = _equipment.PlayerSword;
            //storageCommand.Parameters["@harness"].Value = _equipment.PlayerHarness;
            //storageCommand.Parameters["@spear"].Value = _equipment.PlayerSpear;
            //storageCommand.Parameters["@bow"].Value = _equipment.PlayerBow;
            //storageCommand.Parameters["@gear"].Value = _equipment.PlayerGear;
            //storageCommand.Parameters["@player_id"].Value = player.PlayerId;
            storageCommand.ExecuteNonQuery();


            storageCommand.Dispose();
        }
    }
}
