using LandConquestDB.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandConquestDB.Models
{
    public class CastleModel
    {
        public static void InsertOrUpdateCastle(int landId)
        {
            // это пример, нужно дописать

            string manufactureQuery = "IF EXISTS (SELECT * FROM dbo.PlayerLandManufactureData WHERE player_id = @player_id AND manufacture_id = @manufacture_id) BEGIN UPDATE dbo.PlayerLandManufactureData SET manufacture_peasant_work = @manufacture_peasant_work, manufacture_products_hour = @manufacture_products_hour, manufacture_prod_start_time=@manufacture_prod_start_time WHERE manufacture_id=@manufacture_id AND player_id=@player_id END ELSE BEGIN INSERT INTO dbo.PlayerLandManufactureData (player_id,manufacture_id,manufacture_type,manufacture_peasant_work,manufacture_products_hour,manufacture_prod_start_time) VALUES (@player_id, @manufacture_id, @manufacture_type, @manufacture_peasant_work, @manufacture_products_hour, @manufacture_prod_start_time) END";
        }

        public static Castle GetCastleInfo(int landId)
        {
            Castle castle = new Castle();
            string query = "SELECT * FROM dbo.CastleData where land_id = @land_id";

            var command = new SqlCommand(query, DbContext.GetSqlConnection());
            command.Parameters.AddWithValue("@land_id", landId);

            using (var reader = command.ExecuteReader())
            {
                var castleLandId = reader.GetOrdinal("land_id");
                var castleLvl = reader.GetOrdinal("castle_lvl");
                var landSlotCount = reader.GetOrdinal("castle_slot_count");

                while (reader.Read())
                {
                    castle.LandId = reader.GetInt32(castleLandId);
                    castle.CastleLvl = reader.GetInt32(castleLvl);
                    castle.SlotsCount = reader.GetInt32(landSlotCount);
                }
                reader.Close();
            }

            command.Dispose();

            return castle;
        }

    }

}
