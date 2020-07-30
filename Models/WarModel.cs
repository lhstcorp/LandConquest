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
    }
}
