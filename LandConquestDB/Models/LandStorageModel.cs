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
        public static LandStorage GetCountryStorage(Country country)
        {
            LandStorage countryStorage = new LandStorage();
            string storageQuery = "SELECT * FROM dbo.CountryStorage WHERE country_id = @country_id";

            var command = new SqlCommand(storageQuery, DbContext.GetSqlConnection());
            command.Parameters.AddWithValue("@country_id", country.CountryId);

            using (var reader = command.ExecuteReader())
            {
                var countryId = reader.GetOrdinal("country_id");
                var countryWood = reader.GetOrdinal("wood");
                var countryStone = reader.GetOrdinal("stone");
                var countryFood = reader.GetOrdinal("food");
                var countryIron = reader.GetOrdinal("iron");
                var countryGoldOre = reader.GetOrdinal("gold_ore");
                var countryCopper = reader.GetOrdinal("copper");
                var countryGems = reader.GetOrdinal("gems");
                var countryLeather = reader.GetOrdinal("leather");
                while (reader.Read())
                {
                    countryStorage.LandId = reader.GetString(countryId);
                    countryStorage.LandWood = reader.GetInt32(countryWood);
                    countryStorage.LandStone = reader.GetInt32(countryStone);
                    countryStorage.LandFood = reader.GetInt32(countryFood);
                    countryStorage.LandIron = reader.GetInt32(countryIron);
                    countryStorage.LandGoldOre = reader.GetInt32(countryGoldOre);
                    countryStorage.LandCopper = reader.GetInt32(countryCopper);
                    countryStorage.LandGems = reader.GetInt32(countryGems);
                    countryStorage.LandLeather = reader.GetInt32(countryLeather);
                }
            }

            command.Dispose();
            return countryStorage;
        }

        public static void UpdateStorage(Country country, LandStorage _countryStorage)
        {
            string storageQuery = "UPDATE dbo.StorageData SET wood = @wood, stone  = @stone, food = @food, gold_ore = @gold_ore, copper = @copper, gems = @gems, iron = @iron, leather = @leather WHERE country_id = @country_id ";

            var countryStorageCommand = new SqlCommand(storageQuery, DbContext.GetSqlConnection());
            // int datetimeResult;
            countryStorageCommand.Parameters.AddWithValue("@wood", _countryStorage.LandWood);
            countryStorageCommand.Parameters.AddWithValue("@stone", _countryStorage.LandStone);
            countryStorageCommand.Parameters.AddWithValue("@food", _countryStorage.LandFood);
            countryStorageCommand.Parameters.AddWithValue("@copper", _countryStorage.LandCopper);
            countryStorageCommand.Parameters.AddWithValue("@iron", _countryStorage.LandIron);
            countryStorageCommand.Parameters.AddWithValue("@gems", _countryStorage.LandGems);
            countryStorageCommand.Parameters.AddWithValue("@gold_ore", _countryStorage.LandGoldOre);
            countryStorageCommand.Parameters.AddWithValue("@leather", _countryStorage.LandLeather);
            countryStorageCommand.Parameters.AddWithValue("@country_id", country.CountryId);


            for (int i = 0; i < 3; i++)
            {

                countryStorageCommand.Parameters["@wood"].Value = _countryStorage.LandWood;
                countryStorageCommand.Parameters["@stone"].Value = _countryStorage.LandStone;
                countryStorageCommand.Parameters["@food"].Value = _countryStorage.LandFood;
                countryStorageCommand.Parameters["@player_id"].Value = country.CountryId;
                countryStorageCommand.ExecuteNonQuery();

            }

            countryStorageCommand.Dispose();
        }
    }
}
