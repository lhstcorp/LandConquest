using LandConquest.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandConquest.Models
{
    class WarModel
    {
        public void DeclareAWar(SqlConnection connection, String war_id, Land landAttacker, Land landDefender)
        {
            String Query = "INSERT INTO dbo.WarData (war_id, land_attacker_id, land_defender_id, datetime_start) VALUES (@war_id, @land_attacker_id, @land_defender_id, @datetime_start)";

            var Command = new SqlCommand(Query, connection);
            // int datetimeResult;
            Command.Parameters.AddWithValue("@war_id", war_id);
            Command.Parameters.AddWithValue("@land_attacker_id", landAttacker.LandId);
            Command.Parameters.AddWithValue("@land_defender_id", landDefender.LandId);
            Command.Parameters.AddWithValue("@datetime_start", DateTime.UtcNow);

            Command.ExecuteNonQuery();

            Command.Dispose();
        }

        public int SelectLastIdOfWars(SqlConnection connection)
        {
            String stateQuery = "SELECT * FROM dbo.WarData ORDER BY war_id DESC";
            var stateCommand = new SqlCommand(stateQuery, connection);
            string state_max_id = "";
            int count = 0;
            using (var reader = stateCommand.ExecuteReader())
            {
                var stateId = reader.GetOrdinal("war_id");
                while (reader.Read())
                {
                    state_max_id = reader.GetString(stateId);
                    count++;
                }
            }

            stateCommand.Dispose();
            return count;
        }

        public List<War> GetWarsInfo(List<War> wars, SqlConnection connection)
        {
            String query = "SELECT * FROM dbo.WarData";
            List<String> warssWarId = new List<String>();
            List<Int32> warsLandAttackerId = new List<Int32>();
            List<Int32> warsLandDefenderId = new List<Int32>();
            List<DateTime> warsWarDateTimeStart = new List<DateTime>();

            var command = new SqlCommand(query, connection);

            using (var reader = command.ExecuteReader())
            {
                var warId = reader.GetOrdinal("war_id");
                var landAttackerId = reader.GetOrdinal("land_attacker_id");
                var landDefenderId = reader.GetOrdinal("land_defender_id");
                var warDateTimeStart = reader.GetOrdinal("datetime_start");

                while (reader.Read())
                {
                    warssWarId.Add(reader.GetString(warId));
                    warsLandAttackerId.Add(reader.GetInt32(landAttackerId));
                    warsLandDefenderId.Add(reader.GetInt32(landDefenderId));
                    warsWarDateTimeStart.Add(reader.GetDateTime(warDateTimeStart));
                }
            }

            command.Dispose();

            for (int i = 0; i < warssWarId.Count; i++)
            {
                wars[i].WarId = warssWarId[i];
                wars[i].LandAttackerId = warsLandAttackerId[i];
                wars[i].LandDefenderId = warsLandDefenderId[i];
                wars[i].WarDateTimeStart = warsWarDateTimeStart[i];
            }

            warssWarId = null;
            warsLandAttackerId = null;
            warsLandDefenderId = null;
            warsWarDateTimeStart = null;

            return wars;
        }
    }
}
