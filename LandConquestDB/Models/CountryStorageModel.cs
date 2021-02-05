using LandConquestDB.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandConquestDB.Models
{
    public class CountryStorageModel
    {
        public static CountryStorage GetCountryStorage(Country country)
        {
            CountryStorage countryStorage = new CountryStorage();
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
                    countryStorage.CountryId = reader.GetString(countryId);
                    countryStorage.CountryWood = reader.GetInt32(countryWood);
                    countryStorage.CountryStone = reader.GetInt32(countryStone);
                    countryStorage.CountryFood = reader.GetInt32(countryFood);
                    countryStorage.CountryIron = reader.GetInt32(countryIron);
                    countryStorage.CountryGoldOre = reader.GetInt32(countryGoldOre);
                    countryStorage.CountryCopper = reader.GetInt32(countryCopper);
                    countryStorage.CountryGems = reader.GetInt32(countryGems);
                    countryStorage.CountryLeather = reader.GetInt32(countryLeather);
                }
            }

            command.Dispose();
            return countryStorage;
        }

        public static void UpdateStorage(Country country, CountryStorage _countryStorage)
        {
            string storageQuery = "UPDATE dbo.StorageData SET wood = @wood, stone  = @stone, food = @food, gold_ore = @gold_ore, copper = @copper, gems = @gems, iron = @iron, leather = @leather WHERE country_id = @country_id ";

            var countryStorageCommand = new SqlCommand(storageQuery, DbContext.GetSqlConnection());
            // int datetimeResult;
            countryStorageCommand.Parameters.AddWithValue("@wood", _countryStorage.CountryWood);
            countryStorageCommand.Parameters.AddWithValue("@stone", _countryStorage.CountryStone);
            countryStorageCommand.Parameters.AddWithValue("@food", _countryStorage.CountryFood);
            countryStorageCommand.Parameters.AddWithValue("@copper", _countryStorage.CountryCopper);
            countryStorageCommand.Parameters.AddWithValue("@iron", _countryStorage.CountryIron);
            countryStorageCommand.Parameters.AddWithValue("@gems", _countryStorage.CountryGems);
            countryStorageCommand.Parameters.AddWithValue("@gold_ore", _countryStorage.CountryGoldOre);
            countryStorageCommand.Parameters.AddWithValue("@leather", _countryStorage.CountryLeather);
            countryStorageCommand.Parameters.AddWithValue("@country_id", country.CountryId);


            for (int i = 0; i < 3; i++)
            {

                countryStorageCommand.Parameters["@wood"].Value = _countryStorage.CountryWood;
                countryStorageCommand.Parameters["@stone"].Value = _countryStorage.CountryStone;
                countryStorageCommand.Parameters["@food"].Value = _countryStorage.CountryFood;
                countryStorageCommand.Parameters["@player_id"].Value = country.CountryId;
                countryStorageCommand.ExecuteNonQuery();

            }

            countryStorageCommand.Dispose();
        }
    }
}
