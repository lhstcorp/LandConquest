using LandConquest.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandConquest.Models
{
    public class ManufactureModel
    {
        List<Manufacture> manufactures;

        public DateTime GetManufactureProdStartTime(Player player, SqlConnection connection)
        {
            String query = "SELECT * FROM dbo.ManufactureData WHERE player_id = @player_id";
            DateTime dateTime = new DateTime();

            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@player_id", player.PlayerId);

            using (var reader = command.ExecuteReader())
            {
                var playerId = reader.GetOrdinal("manufacture_prod_start_time");


                while (reader.Read())
                {
                    dateTime = reader.GetDateTime(playerId);
                }
            }

            return dateTime;
        }

        public List<Manufacture> GetManufactureInfo(Player player, SqlConnection connection)
        {
            //List<Manufacture> manufactures = new List<Manufacture>();

            String query = "SELECT * FROM dbo.ManufactureData WHERE player_id = @player_id";
            List<string> manufacturesPlayerId = new List<string>();
            List<string> manufacturesManufactureId = new List<string>();
            List<string> manufacturesManufactureName = new List<string>();
            List<Int32> manufacturesManufactureType = new List<Int32>();
            List<Int32> manufacturesManufactureLevel = new List<Int32>();
            List<Int32> manufacturesManufacturePeasantsMax = new List<Int32>();
            List<Int32> manufacturesManufacturePeasantsWork = new List<Int32>();
            List<Int32> manufacturesManufactureProductsHour = new List<Int32>();
            List<DateTime> manufacturesManufactureProdStartTime = new List<DateTime>();
            List<Int32> manufacturesManufactureBaseProdValue = new List<Int32>();

            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@player_id", player.PlayerId);

            using (var reader = command.ExecuteReader())
            {
                var playerId = reader.GetOrdinal("player_id");
                var manufactureId = reader.GetOrdinal("manufacture_id");
                var manufactureName = reader.GetOrdinal("manufacture_name");
                var manufactureType = reader.GetOrdinal("manufacture_type");
                var manufactureLevel = reader.GetOrdinal("manufacture_lvl");
                var manufacturePeasantsMax = reader.GetOrdinal("manufacture_peasant_max");
                var manufacturePeasantsWork = reader.GetOrdinal("manufacture_peasant_work");
                var manufactureProductsHour = reader.GetOrdinal("manufacture_products_hour");
                var manufactureProdStartTime = reader.GetOrdinal("manufacture_prod_start_time");
                var manufactureBaseProdValue = reader.GetOrdinal("manufacture_base_prod_value");

                while (reader.Read())
                {
                    manufacturesPlayerId.Add(reader.GetString(playerId));
                    manufacturesManufactureId.Add(reader.GetString(manufactureId));
                    //Console.WriteLine(reader.GetString(manufactureId));
                    manufacturesManufactureName.Add(reader.GetString(manufactureName));
                    manufacturesManufactureType.Add(reader.GetInt32(manufactureType));
                    manufacturesManufactureLevel.Add(reader.GetInt32(manufactureLevel));
                    manufacturesManufacturePeasantsMax.Add(reader.GetInt32(manufacturePeasantsMax));
                    manufacturesManufacturePeasantsWork.Add(reader.GetInt32(manufacturePeasantsWork));
                    manufacturesManufactureProductsHour.Add(reader.GetInt32(manufactureProductsHour));
                    manufacturesManufactureProdStartTime.Add(reader.GetDateTime(manufactureProdStartTime));
                    manufacturesManufactureBaseProdValue.Add(reader.GetInt32(manufactureBaseProdValue));
                }


            }

            command.Dispose();

            manufactures = new List<Manufacture>();
            for (int i = 0; i < manufacturesPlayerId.Count; i++)
            {
                manufactures.Add(new Manufacture());
            }

            for (int i = 0; i < manufacturesPlayerId.Count; i++)
            {
                Console.WriteLine(manufacturesManufactureName[i]);
                manufactures[i].PlayerId = manufacturesPlayerId[i];
                manufactures[i].ManufactureId = manufacturesManufactureId[i];
                manufactures[i].ManufactureName = manufacturesManufactureName[i];
                Console.WriteLine(i);
                manufactures[i].ManufactureType = manufacturesManufactureType[i];
                manufactures[i].ManufactureLevel = manufacturesManufactureLevel[i];
                manufactures[i].ManufacturePeasantMax = manufacturesManufacturePeasantsMax[i];
                manufactures[i].ManufacturePeasantWork = manufacturesManufacturePeasantsWork[i];
                manufactures[i].ManufactureProductsHour = manufacturesManufactureProductsHour[i];
                manufactures[i].ManufactureProdStartTime = manufacturesManufactureProdStartTime[i];
                manufactures[i].ManufactureBaseProdValue = manufacturesManufactureBaseProdValue[i];
            }

            manufacturesPlayerId = null;
            manufacturesManufactureName = null;
            manufacturesManufactureType = null;
            manufacturesManufactureLevel = null;
            manufacturesManufacturePeasantsMax = null;
            manufacturesManufacturePeasantsWork = null;
            manufacturesManufactureProductsHour = null;
            manufacturesManufactureProdStartTime = null;
            manufacturesManufactureBaseProdValue = null;

            return manufactures;
        }

        public List<Manufacture> GetLandManufactureInfo(Player player, SqlConnection connection)
        {
            //List<Manufacture> manufactures = new List<Manufacture>();

            String query = "SELECT * FROM dbo.LandManufactureData WHERE land_id = @land_id";
            List<int> manufacturesLandId = new List<int>();
            List<string> manufacturesManufactureId = new List<string>();
            List<string> manufacturesManufactureName = new List<string>();
            List<Int32> manufacturesManufactureType = new List<Int32>();
            List<Int32> manufacturesManufactureLevel = new List<Int32>();
            List<Int32> manufacturesManufacturePeasantsMax = new List<Int32>();
            List<Int32> manufacturesManufacturePeasantsWork = new List<Int32>();
            List<Int32> manufacturesManufactureProductsHour = new List<Int32>();
            List<DateTime> manufacturesManufactureProdStartTime = new List<DateTime>();
            List<Int32> manufacturesManufactureBaseProdValue = new List<Int32>();

            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@land_id", player.PlayerCurrentRegion);

            using (var reader = command.ExecuteReader())
            {
                var landId = reader.GetOrdinal("land_id");
                var manufactureId = reader.GetOrdinal("manufacture_id");
                var manufactureName = reader.GetOrdinal("manufacture_name");
                var manufactureType = reader.GetOrdinal("manufacture_type");
                var manufactureLevel = reader.GetOrdinal("manufacture_lvl");
                var manufacturePeasantsMax = reader.GetOrdinal("manufacture_peasant_max");
                var manufacturePeasantsWork = reader.GetOrdinal("manufacture_peasant_work");
                var manufactureProductsHour = reader.GetOrdinal("manufacture_products_hour");
                var manufactureProdStartTime = reader.GetOrdinal("manufacture_prod_start_time");
                var manufactureBaseProdValue = reader.GetOrdinal("manufacture_base_prod_value");

                while (reader.Read())
                {
                    manufacturesLandId.Add(reader.GetInt32(landId));
                    manufacturesManufactureId.Add(reader.GetString(manufactureId));
                    manufacturesManufactureName.Add(reader.GetString(manufactureName));
                    manufacturesManufactureType.Add(reader.GetInt32(manufactureType));
                    manufacturesManufactureLevel.Add(reader.GetInt32(manufactureLevel));
                    manufacturesManufacturePeasantsMax.Add(reader.GetInt32(manufacturePeasantsMax));
                    manufacturesManufacturePeasantsWork.Add(reader.GetInt32(manufacturePeasantsWork));
                    manufacturesManufactureProductsHour.Add(reader.GetInt32(manufactureProductsHour));
                    manufacturesManufactureProdStartTime.Add(reader.GetDateTime(manufactureProdStartTime));
                    manufacturesManufactureBaseProdValue.Add(reader.GetInt32(manufactureBaseProdValue));
                }
            }

            command.Dispose();

            manufactures = new List<Manufacture>();
            for (int i = 0; i < manufacturesLandId.Count; i++)
            {
                manufactures.Add(new Manufacture());
            }

            for (int i = 0; i < manufacturesLandId.Count; i++)
            {
                Console.WriteLine(manufacturesManufactureName[i]);
                manufactures[i].PlayerId = manufacturesLandId[i].ToString();
                manufactures[i].ManufactureId = manufacturesManufactureId[i];
                manufactures[i].ManufactureName = manufacturesManufactureName[i];
                Console.WriteLine(i);
                manufactures[i].ManufactureType = manufacturesManufactureType[i];
                manufactures[i].ManufactureLevel = manufacturesManufactureLevel[i];
                manufactures[i].ManufacturePeasantMax = manufacturesManufacturePeasantsMax[i];
                manufactures[i].ManufacturePeasantWork = manufacturesManufacturePeasantsWork[i];
                manufactures[i].ManufactureProductsHour = manufacturesManufactureProductsHour[i];
                manufactures[i].ManufactureProdStartTime = manufacturesManufactureProdStartTime[i];
                manufactures[i].ManufactureBaseProdValue = manufacturesManufactureBaseProdValue[i];
            }

            manufacturesLandId = null;
            manufacturesManufactureName = null;
            manufacturesManufactureType = null;
            manufacturesManufactureLevel = null;
            manufacturesManufacturePeasantsMax = null;
            manufacturesManufacturePeasantsWork = null;
            manufacturesManufactureProductsHour = null;
            manufacturesManufactureProdStartTime = null;
            manufacturesManufactureBaseProdValue = null;

            return manufactures;
        }



        public void UpdateDateTimeForManufacture(List<Manufacture> _manufactures, Player _player, SqlConnection connection)
        {
            String datetimeQuery = "UPDATE dbo.ManufactureData SET manufacture_peasant_work  = @manufacture_peasant_work, manufacture_products_hour  = @manufacture_products_hour, manufacture_prod_start_time = @manufacture_prod_start_time WHERE manufacture_id = @manufacture_id ";

            var datetimeCommand = new SqlCommand(datetimeQuery, connection);
            // int datetimeResult;
            datetimeCommand.Parameters.AddWithValue("@manufacture_peasant_work", _manufactures[0].ManufacturePeasantWork);
            datetimeCommand.Parameters.AddWithValue("@manufacture_products_hour", _manufactures[0].ManufactureProductsHour);
            datetimeCommand.Parameters.AddWithValue("@manufacture_prod_start_time", DateTime.UtcNow);
            datetimeCommand.Parameters.AddWithValue("@manufacture_id", _manufactures[0].ManufactureId);

            for (int i = 0; i < 3; i++)
            {

                datetimeCommand.Parameters["@manufacture_peasant_work"].Value = _manufactures[i].ManufacturePeasantWork;
                datetimeCommand.Parameters["@manufacture_products_hour"].Value = _manufactures[i].ManufactureProductsHour;
                datetimeCommand.Parameters["@manufacture_prod_start_time"].Value = DateTime.UtcNow;
                datetimeCommand.Parameters["@manufacture_id"].Value = _manufactures[i].ManufactureId;
                datetimeCommand.ExecuteNonQuery();
                Console.WriteLine(_manufactures[i].ManufactureId);

            }

            datetimeCommand.Dispose();

        }

        public void UpdateDateTimeForPlayerLandManufacture(List<Manufacture> _manufactures, Player _player, SqlConnection connection)
        {
            String datetimeQuery = "UPDATE dbo.PlayerLandManufactureData SET manufacture_prod_start_time = @manufacture_prod_start_time WHERE manufacture_id = @manufacture_id ";

            var datetimeCommand1 = new SqlCommand(datetimeQuery, connection);
            datetimeCommand1.Parameters.AddWithValue("@manufacture_prod_start_time", DateTime.UtcNow);
            datetimeCommand1.Parameters.AddWithValue("@manufacture_id", _manufactures[0].ManufactureId);

            datetimeCommand1.ExecuteNonQuery();

            var datetimeCommand2 = new SqlCommand(datetimeQuery, connection);

            datetimeCommand2.Parameters.AddWithValue("@manufacture_prod_start_time", DateTime.UtcNow);
            datetimeCommand2.Parameters.AddWithValue("@manufacture_id", _manufactures[1].ManufactureId);

            datetimeCommand2.ExecuteNonQuery();

            datetimeCommand1.Dispose();
            datetimeCommand2.Dispose();

        }

        public PlayerStorage GetInfoAboutResourcesForUpdate(SqlConnection connection, Manufacture manufacture)
        {
            PlayerStorage resourcesNeed = new PlayerStorage();
            resourcesNeed.PlayerId = null;
            resourcesNeed.PlayerFood = 0;

            String query = "SELECT * FROM dbo.ManufactureLvlData WHERE lvl = @manufacture_lvl";

            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@manufacture_lvl", manufacture.ManufactureLevel);

            using (var reader = command.ExecuteReader())
            {
                var woodNeeded = reader.GetOrdinal("wood");
                var stoneNeeded = reader.GetOrdinal("stone");


                while (reader.Read())
                {
                    resourcesNeed.PlayerWood = reader.GetInt32(woodNeeded);
                    resourcesNeed.PlayerStone = reader.GetInt32(stoneNeeded);
                }
            }

            return resourcesNeed;
        }

        public void UpgradeManufacture(SqlConnection connection, Manufacture manufacture)
        {
            String manufactureUpgradeQuery = "UPDATE dbo.ManufactureData SET manufacture_lvl = @manufacture_lvl, manufacture_peasant_max  = @manufacture_peasant_max, manufacture_products_hour = @manufacture_products_hour, manufacture_base_prod_value = @manufacture_base_prod_value WHERE manufacture_id = @manufacture_id ";

            var manufactureUpgradeCommand = new SqlCommand(manufactureUpgradeQuery, connection);

            if (manufacture.ManufactureLevel % 2 == 0)
            {
                manufacture.ManufactureBaseProdValue -= 1;
            }
            else
            {
                manufacture.ManufacturePeasantMax -= 200;
            }

            manufactureUpgradeCommand.Parameters.AddWithValue("@manufacture_lvl", manufacture.ManufactureLevel + 1);
            manufactureUpgradeCommand.Parameters.AddWithValue("@manufacture_peasant_max", manufacture.ManufacturePeasantMax + 200);
            manufactureUpgradeCommand.Parameters.AddWithValue("@manufacture_products_hour", (manufacture.ManufactureBaseProdValue + 1) * manufacture.ManufacturePeasantWork);
            manufactureUpgradeCommand.Parameters.AddWithValue("@manufacture_base_prod_value", manufacture.ManufactureBaseProdValue + 1);
            manufactureUpgradeCommand.Parameters.AddWithValue("@manufacture_id", manufacture.ManufactureId);

            manufactureUpgradeCommand.ExecuteNonQuery();

            manufactureUpgradeCommand.Dispose();
        }

        public Manufacture GetManufactureById(Manufacture _manufacture, SqlConnection connection)
        {
            Manufacture manufacture = new Manufacture();
            String query = "SELECT * FROM dbo.ManufactureData WHERE manufacture_id = @manufacture_id";

            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@manufacture_id", _manufacture.ManufactureId);

            using (var reader = command.ExecuteReader())
            {
                var playerId = reader.GetOrdinal("player_id");
                var manufactureName = reader.GetOrdinal("manufacture_name");
                var manufactureType = reader.GetOrdinal("manufacture_type");
                var manufactureLevel = reader.GetOrdinal("manufacture_lvl");
                var manufacturePeasantsMax = reader.GetOrdinal("manufacture_peasant_max");
                var manufacturePeasantsWork = reader.GetOrdinal("manufacture_peasant_work");
                var manufactureProductsHour = reader.GetOrdinal("manufacture_products_hour");
                var manufactureProdStartTime = reader.GetOrdinal("manufacture_prod_start_time");
                var manufactureBaseProdValue = reader.GetOrdinal("manufacture_base_prod_value");


                while (reader.Read())
                {
                    manufacture.PlayerId = reader.GetString(playerId);
                    manufacture.ManufactureName = reader.GetString(manufactureName);
                    manufacture.ManufactureType = reader.GetInt32(manufactureType);
                    manufacture.ManufactureLevel = reader.GetInt32(manufactureLevel);
                    manufacture.ManufacturePeasantMax = reader.GetInt32(manufacturePeasantsMax);
                    manufacture.ManufacturePeasantWork = reader.GetInt32(manufacturePeasantsWork);
                    manufacture.ManufactureProductsHour = reader.GetInt32(manufactureProductsHour);
                    manufacture.ManufactureProdStartTime = reader.GetDateTime(manufactureProdStartTime);
                    manufacture.ManufactureBaseProdValue = reader.GetInt32(manufactureBaseProdValue);
                }
            }

            manufacture.ManufactureId = _manufacture.ManufactureId;

            return manufacture;
        }

        public void InsertOrUpdateLandManufactures(List<Manufacture> landManufactures, Player player, SqlConnection connection)
        {
            String manufactureQuery = "IF EXISTS (SELECT * FROM dbo.PlayerLandManufactureData WHERE player_id = @player_id AND manufacture_id = @manufacture_id) BEGIN UPDATE dbo.PlayerLandManufactureData SET manufacture_peasant_work = @manufacture_peasant_work, manufacture_products_hour = @manufacture_products_hour, manufacture_prod_start_time=@manufacture_prod_start_time WHERE manufacture_id=@manufacture_id AND player_id=@player_id END ELSE BEGIN INSERT INTO dbo.PlayerLandManufactureData (player_id,manufacture_id,manufacture_type,manufacture_peasant_work,manufacture_products_hour,manufacture_prod_start_time) VALUES (@player_id, @manufacture_id, @manufacture_type, @manufacture_peasant_work, @manufacture_products_hour, @manufacture_prod_start_time) END";

            //b1
            var build1Command = new SqlCommand(manufactureQuery, connection);


            build1Command.Parameters.AddWithValue("@player_id", player.PlayerId);
            build1Command.Parameters.AddWithValue("@manufacture_id", landManufactures[0].ManufactureId);
            build1Command.Parameters.AddWithValue("@manufacture_type", landManufactures[0].ManufactureType);
            build1Command.Parameters.AddWithValue("@manufacture_peasant_work", landManufactures[0].ManufacturePeasantWork);
            build1Command.Parameters.AddWithValue("@manufacture_products_hour", landManufactures[0].ManufactureProductsHour);
            build1Command.Parameters.AddWithValue("@manufacture_prod_start_time", DateTime.UtcNow);

            build1Command.ExecuteNonQuery();

            //b2
            var build2Command = new SqlCommand(manufactureQuery, connection);

            Console.WriteLine("manufacture_bv = " + landManufactures[1].ManufactureBaseProdValue);

            build2Command.Parameters.AddWithValue("@player_id", player.PlayerId);
            build2Command.Parameters.AddWithValue("@manufacture_id", landManufactures[1].ManufactureId);
            build2Command.Parameters.AddWithValue("@manufacture_type", landManufactures[1].ManufactureType);
            build2Command.Parameters.AddWithValue("@manufacture_peasant_work", landManufactures[1].ManufacturePeasantWork);
            build2Command.Parameters.AddWithValue("@manufacture_products_hour", landManufactures[1].ManufactureProductsHour);
            build2Command.Parameters.AddWithValue("@manufacture_prod_start_time", DateTime.UtcNow);

            build2Command.ExecuteNonQuery();

            build1Command.Dispose();
            build2Command.Dispose();
        }

        public void UpdateLandManufactures(List<Manufacture> landManufactures, SqlConnection connection)
        {
            String manufactureQuery = "UPDATE dbo.LandManufactureData SET manufacture_peasant_work  = @manufacture_peasant_work, manufacture_products_hour  = @manufacture_products_hour WHERE manufacture_id = @manufacture_id ";

            //b1
            var build1Command = new SqlCommand(manufactureQuery, connection);

            build1Command.Parameters.AddWithValue("@manufacture_id", landManufactures[0].ManufactureId);
            build1Command.Parameters.AddWithValue("@manufacture_peasant_work", landManufactures[0].ManufacturePeasantWork);
            build1Command.Parameters.AddWithValue("@manufacture_products_hour", landManufactures[0].ManufactureProductsHour);

            build1Command.ExecuteNonQuery();

            //b2
            var build2Command = new SqlCommand(manufactureQuery, connection);

            build2Command.Parameters.AddWithValue("@manufacture_id", landManufactures[1].ManufactureId);
            build2Command.Parameters.AddWithValue("@manufacture_peasant_work", landManufactures[1].ManufacturePeasantWork);
            build2Command.Parameters.AddWithValue("@manufacture_products_hour", landManufactures[1].ManufactureProductsHour);

            build2Command.ExecuteNonQuery();

            build1Command.Dispose();
            build2Command.Dispose();

        }

        public List<Manufacture> GetPlayerLandManufactureInfo(Player player, SqlConnection connection)
        {
            //List<Manufacture> manufactures = new List<Manufacture>();

            String query = "SELECT * FROM dbo.PlayerLandManufactureData WHERE player_id = @player_id";
            List<string> manufacturesPlayerId = new List<string>();
            List<string> manufacturesManufactureId = new List<string>();
            //List<string> manufacturesManufactureName = new List<string>();
            List<Int32> manufacturesManufactureType = new List<Int32>();
            //List<Int32> manufacturesManufactureLevel = new List<Int32>();
            //List<Int32> manufacturesManufacturePeasantsMax = new List<Int32>();
            List<Int32> manufacturesManufacturePeasantsWork = new List<Int32>();
            List<Int32> manufacturesManufactureProductsHour = new List<Int32>();
            List<DateTime> manufacturesManufactureProdStartTime = new List<DateTime>();
            //List<Int32> manufacturesManufactureBaseProdValue = new List<Int32>();

            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@player_id", player.PlayerId);

            using (var reader = command.ExecuteReader())
            {
                var playerId = reader.GetOrdinal("player_id");
                var manufactureId = reader.GetOrdinal("manufacture_id");
                //var manufactureName = reader.GetOrdinal("manufacture_name");
                var manufactureType = reader.GetOrdinal("manufacture_type");
                //var manufactureLevel = reader.GetOrdinal("manufacture_lvl");
                //var manufacturePeasantsMax = reader.GetOrdinal("manufacture_peasant_max");
                var manufacturePeasantsWork = reader.GetOrdinal("manufacture_peasant_work");
                var manufactureProductsHour = reader.GetOrdinal("manufacture_products_hour");
                var manufactureProdStartTime = reader.GetOrdinal("manufacture_prod_start_time");
                //var manufactureBaseProdValue = reader.GetOrdinal("manufacture_base_prod_value");

                while (reader.Read())
                {
                    manufacturesPlayerId.Add(reader.GetString(playerId));
                    manufacturesManufactureId.Add(reader.GetString(manufactureId));
                    //Console.WriteLine(reader.GetString(manufactureId));
                    //manufacturesManufactureName.Add(reader.GetString(manufactureName));
                    manufacturesManufactureType.Add(reader.GetInt32(manufactureType));
                    //manufacturesManufactureLevel.Add(reader.GetInt32(manufactureLevel));
                    //manufacturesManufacturePeasantsMax.Add(reader.GetInt32(manufacturePeasantsMax));
                    manufacturesManufacturePeasantsWork.Add(reader.GetInt32(manufacturePeasantsWork));
                    manufacturesManufactureProductsHour.Add(reader.GetInt32(manufactureProductsHour));
                    manufacturesManufactureProdStartTime.Add(reader.GetDateTime(manufactureProdStartTime));
                    //manufacturesManufactureBaseProdValue.Add(reader.GetInt32(manufactureBaseProdValue));
                }


            }

            command.Dispose();

            manufactures = new List<Manufacture>();
            for (int i = 0; i < manufacturesPlayerId.Count; i++)
            {
                manufactures.Add(new Manufacture());
            }

            for (int i = 0; i < manufacturesPlayerId.Count; i++)
            {
                //Console.WriteLine(manufacturesManufactureName[i]);
                manufactures[i].PlayerId = manufacturesPlayerId[i];
                manufactures[i].ManufactureId = manufacturesManufactureId[i];
                //manufactures[i].ManufactureName = manufacturesManufactureName[i];
                //Console.WriteLine(i);
                manufactures[i].ManufactureType = manufacturesManufactureType[i];
                //manufactures[i].ManufactureLevel = manufacturesManufactureLevel[i];
                //manufactures[i].ManufacturePeasantMax = manufacturesManufacturePeasantsMax[i];
                manufactures[i].ManufacturePeasantWork = manufacturesManufacturePeasantsWork[i];
                manufactures[i].ManufactureProductsHour = manufacturesManufactureProductsHour[i];
                manufactures[i].ManufactureProdStartTime = manufacturesManufactureProdStartTime[i];
                //manufactures[i].ManufactureBaseProdValue = manufacturesManufactureBaseProdValue[i];
            }

            manufacturesPlayerId = null;
            //manufacturesManufactureName = null;
            manufacturesManufactureType = null;
            //manufacturesManufactureLevel = null;
            //manufacturesManufacturePeasantsMax = null;
            manufacturesManufacturePeasantsWork = null;
            manufacturesManufactureProductsHour = null;
            manufacturesManufactureProdStartTime = null;
            //manufacturesManufactureBaseProdValue = null;

            return manufactures;
        }

        public void UpdateLandManufacturesWhenMove(SqlConnection connection, List<int> list, List<Manufacture> landManufactures)
        {
            String manufactureQuery = "UPDATE dbo.LandManufactureData SET manufacture_peasant_work  = @manufacture_peasant_work WHERE manufacture_id = @manufacture_id ";

            //b1
            var build1Command = new SqlCommand(manufactureQuery, connection);

            build1Command.Parameters.AddWithValue("@manufacture_id", landManufactures[0].ManufactureId);
            build1Command.Parameters.AddWithValue("@manufacture_peasant_work", landManufactures[0].ManufacturePeasantWork - list[0]);

            build1Command.ExecuteNonQuery();

            //b2
            var build2Command = new SqlCommand(manufactureQuery, connection);

            build2Command.Parameters.AddWithValue("@manufacture_id", landManufactures[1].ManufactureId);
            build2Command.Parameters.AddWithValue("@manufacture_peasant_work", landManufactures[1].ManufacturePeasantWork - list[1]);

            build2Command.ExecuteNonQuery();

            build1Command.Dispose();
            build2Command.Dispose();
        }
    }
}
