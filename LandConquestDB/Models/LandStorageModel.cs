using LandConquestDB.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandConquestDB.Models
{
    public class LandStorageModel
    {
        public static LandStorage GetLandStorage(Land land)
        {
            LandStorage landStorage = new LandStorage();
            string storageQuery = "SELECT * FROM dbo.LandStorage WHERE land_id = @land_id";

            var command = new SqlCommand(storageQuery, DbContext.GetSqlConnection());
            command.Parameters.AddWithValue("@land_id", land.LandId);

            using (var reader = command.ExecuteReader())
            {
                var landId = reader.GetOrdinal("country_id");
                var landWood = reader.GetOrdinal("wood");
                var landStone = reader.GetOrdinal("stone");
                var landFood = reader.GetOrdinal("food");
                var landIron = reader.GetOrdinal("iron");
                var landGoldOre = reader.GetOrdinal("gold_ore");
                var landCopper = reader.GetOrdinal("copper");
                var landGems = reader.GetOrdinal("gems");
                var landLeather = reader.GetOrdinal("leather");
                while (reader.Read())
                {
                    landStorage.LandId = reader.GetString(landId);
                    landStorage.LandWood = reader.GetInt32(landWood);
                    landStorage.LandStone = reader.GetInt32(landStone);
                    landStorage.LandFood = reader.GetInt32(landFood);
                    landStorage.LandIron = reader.GetInt32(landIron);
                    landStorage.LandGoldOre = reader.GetInt32(landGoldOre);
                    landStorage.LandCopper = reader.GetInt32(landCopper);
                    landStorage.LandGems = reader.GetInt32(landGems);
                    landStorage.LandLeather = reader.GetInt32(landLeather);
                }
                reader.Close();
            }

            command.Dispose();
            return landStorage;
        }

        public static void UpdateLandStorage(Land land, LandStorage _landStorage)
        {
            string storageQuery = "UPDATE dbo.LandStorage SET wood = @wood, stone  = @stone, food = @food, gold_ore = @gold_ore, copper = @copper, gems = @gems, iron = @iron, leather = @leather WHERE land_id = @land_id ";

            var countryStorageCommand = new SqlCommand(storageQuery, DbContext.GetSqlConnection());
            // int datetimeResult;
            countryStorageCommand.Parameters.AddWithValue("@wood", _landStorage.LandWood);
            countryStorageCommand.Parameters.AddWithValue("@stone", _landStorage.LandStone);
            countryStorageCommand.Parameters.AddWithValue("@food", _landStorage.LandFood);
            countryStorageCommand.Parameters.AddWithValue("@copper", _landStorage.LandCopper);
            countryStorageCommand.Parameters.AddWithValue("@iron", _landStorage.LandIron);
            countryStorageCommand.Parameters.AddWithValue("@gems", _landStorage.LandGems);
            countryStorageCommand.Parameters.AddWithValue("@gold_ore", _landStorage.LandGoldOre);
            countryStorageCommand.Parameters.AddWithValue("@leather", _landStorage.LandLeather);
            countryStorageCommand.Parameters.AddWithValue("@country_id", land.LandId);


            for (int i = 0; i < 3; i++)
            {

                countryStorageCommand.Parameters["@wood"].Value = _landStorage.LandWood;
                countryStorageCommand.Parameters["@stone"].Value = _landStorage.LandStone;
                countryStorageCommand.Parameters["@food"].Value = _landStorage.LandFood;
                countryStorageCommand.Parameters["@player_id"].Value = land.LandId;
                countryStorageCommand.ExecuteNonQuery();

            }

            countryStorageCommand.Dispose();
        }


    }
}
