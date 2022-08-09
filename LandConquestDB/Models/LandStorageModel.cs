using Dapper;
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
        public static LandStorage GetLandStorage(Land land, LandStorage landStorage)
        {
            string storageQuery = "SELECT * FROM dbo.LandStorage WHERE land_id = @land_id";
            //return DbContext.GetSqlConnection().Query<LandStorage>("SELECT * FROM dbo.LandStorage WHERE land_id = @land_id", new { land_id = land.LandId }).FirstOrDefault();
            var command = new SqlCommand(storageQuery, DbContext.GetSqlConnection());
            command.Parameters.AddWithValue("@land_id", land.LandId);

            using (var reader = command.ExecuteReader())
            {
                var landId = reader.GetOrdinal("land_id");
                var landWood = reader.GetOrdinal("wood");
                var landStone = reader.GetOrdinal("stone");
                var landFood = reader.GetOrdinal("food");
                var landIron = reader.GetOrdinal("iron");
                var landGoldOre = reader.GetOrdinal("gold_ore");
                var landCopper = reader.GetOrdinal("copper");
                var landGems = reader.GetOrdinal("gems");
                var landLeather = reader.GetOrdinal("leather");
                var landMoney = reader.GetOrdinal("money");
                while (reader.Read())
                {
                    landStorage.LandId = reader.GetString(landId);
                    landStorage.Wood = reader.GetInt32(landWood);
                    landStorage.Stone = reader.GetInt32(landStone);
                    landStorage.Food = reader.GetInt32(landFood);
                    landStorage.Iron = reader.GetInt32(landIron);
                    landStorage.GoldOre = reader.GetInt32(landGoldOre);
                    landStorage.Copper = reader.GetInt32(landCopper);
                    landStorage.Gems = reader.GetInt32(landGems);
                    landStorage.Leather = reader.GetInt32(landLeather);
                    landStorage.Money = reader.GetInt32(landMoney);
                }
                reader.Close();
            }

            command.Dispose();
            return landStorage;
        }

        public static void UpdateLandStorage(Land land, LandStorage _landStorage)
        {
            string storageQuery = "UPDATE dbo.LandStorage SET wood = @wood, stone  = @stone, food = @food, gold_ore = @gold_ore, copper = @copper, gems = @gems, iron = @iron, leather = @leather, money = @money WHERE land_id = @land_id ";

            //DbContext.GetSqlConnection().Execute("UPDATE dbo.LandStorage SET wood = @wood, stone  = @stone, food = @food, gold_ore = @gold_ore, copper = @copper, gems = @gems, iron = @iron, leather = @leather, money = @money WHERE land_id = @land_id ", new { wood = _landStorage.Wood, stone = _landStorage.Stone, food = _landStorage.Food, copper = _landStorage.Copper, iron = _landStorage.Iron, gems = _landStorage.Gems, gold_ore = _landStorage.GoldOre, leather = _landStorage.Leather, money = _landStorage.Money, land_id = land.LandId });

            var countryStorageCommand = new SqlCommand(storageQuery, DbContext.GetSqlConnection());
            // int datetimeResult;
            countryStorageCommand.Parameters.AddWithValue("@wood", _landStorage.Wood);
            countryStorageCommand.Parameters.AddWithValue("@stone", _landStorage.Stone);
            countryStorageCommand.Parameters.AddWithValue("@food", _landStorage.Food);
            countryStorageCommand.Parameters.AddWithValue("@copper", _landStorage.Copper);
            countryStorageCommand.Parameters.AddWithValue("@iron", _landStorage.Iron);
            countryStorageCommand.Parameters.AddWithValue("@gems", _landStorage.Gems);
            countryStorageCommand.Parameters.AddWithValue("@gold_ore", _landStorage.GoldOre);
            countryStorageCommand.Parameters.AddWithValue("@leather", _landStorage.Leather);
            countryStorageCommand.Parameters.AddWithValue("@money", _landStorage.Money);
            countryStorageCommand.Parameters.AddWithValue("@land_id", land.LandId);

            /*
            for (int i = 0; i < 3; i++)
            {

                countryStorageCommand.Parameters["@wood"].Value = _landStorage.LandWood;
                countryStorageCommand.Parameters["@stone"].Value = _landStorage.LandStone;
                countryStorageCommand.Parameters["@food"].Value = _landStorage.LandFood;
                countryStorageCommand.Parameters["@land_id"].Value = land.LandId;
                

            }*/
            countryStorageCommand.ExecuteNonQuery();

            countryStorageCommand.Dispose();
        }


    }
}
