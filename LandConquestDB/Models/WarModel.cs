using LandConquestDB.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
namespace LandConquestDB.Models
{
    public static class WarModel
    {
        public static void DeclareAWar(string war_id, Land landAttacker, Land landDefender)
        {
            string Query = "INSERT INTO dbo.WarData (war_id, land_attacker_id, land_defender_id, datetime_start) VALUES (@war_id, @land_attacker_id, @land_defender_id, @datetime_start)";

            var Command = new SqlCommand(Query, DbContext.GetSqlConnection());
            // int datetimeResult;
            Command.Parameters.AddWithValue("@war_id", war_id);
            Command.Parameters.AddWithValue("@land_attacker_id", landAttacker.LandId);
            Command.Parameters.AddWithValue("@land_defender_id", landDefender.LandId);
            Command.Parameters.AddWithValue("@datetime_start", DateTime.UtcNow);

            Command.ExecuteNonQuery();

            Command.Dispose();
        }

        public static War GetWarById(War war)
        {
            string query = "SELECT * FROM dbo.WarData WHERE war_id = @war_id";
            var command = new SqlCommand(query, DbContext.GetSqlConnection());
            command.Parameters.AddWithValue("@war_id", war.WarId);

            using (var reader = command.ExecuteReader())
            {
                var warId = reader.GetOrdinal("war_id");
                var landAttackerId = reader.GetOrdinal("land_attacker_id");
                var landDefenderId = reader.GetOrdinal("land_defender_id");
                var warDateTimeStart = reader.GetOrdinal("datetime_start");

                while (reader.Read())
                {
                    war.WarId = (reader.GetString(warId));
                    war.LandAttackerId = (reader.GetInt32(landAttackerId));
                    war.LandDefenderId = (reader.GetInt32(landDefenderId));
                    war.WarDateTimeStart = (reader.GetDateTime(warDateTimeStart));
                }
                reader.Close();
            }

            command.Dispose();
            return war;
        }

        public static int SelectLastIdOfWars()
        {
            string stateQuery = "SELECT * FROM dbo.WarData ORDER BY war_id DESC";
            var stateCommand = new SqlCommand(stateQuery, DbContext.GetSqlConnection());
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
                reader.Close();
            }

            stateCommand.Dispose();
            return count;
        }

        public static List<War> GetWarsInfo(List<War> wars)
        {
            string query = "SELECT * FROM dbo.WarData";
            List<string> warssWarId = new List<string>();
            List<int> warsLandAttackerId = new List<int>();
            List<int> warsLandDefenderId = new List<int>();
            List<DateTime> warsWarDateTimeStart = new List<DateTime>();

            var command = new SqlCommand(query, DbContext.GetSqlConnection());

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
                reader.Close();
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
