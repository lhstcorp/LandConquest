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
                    castle.CastleSlotCount = reader.GetInt32(landSlotCount);
                }
                reader.Close();
            }

            command.Dispose();

            return castle;
        }

        public static void UpdateCastle(Castle _castle)
        {
            string Query = "UPDATE dbo.CastleData SET castle_lvl  = @castle_lvl, castle_slot_count = @castle_slot_count WHERE land_id = @land_id ";

            var Command = new SqlCommand(Query, DbContext.GetSqlConnection());

            Command.Parameters.AddWithValue("@land_id", _castle.LandId);
            Command.Parameters.AddWithValue("@castle_lvl", _castle.CastleLvl);
            Command.Parameters.AddWithValue("@castle_slot_count", _castle.CastleSlotCount);

            Command.ExecuteNonQuery();

            Command.Dispose();
        }

    }

}
