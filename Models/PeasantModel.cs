using LandConquest.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandConquest.Models
{
    public class PeasantModel
    {
        public Peasants GetPeasantsInfo(Player player, SqlConnection connection, Peasants peasants)
        {
            String query = "SELECT * FROM dbo.PeasantsData WHERE player_id = @player_id";

            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@player_id", player.PlayerId);

            using (var reader = command.ExecuteReader())
            {
                var playerId = reader.GetOrdinal("player_id");
                var peasantsCount = reader.GetOrdinal("peasants_count");
                var peasantsWork = reader.GetOrdinal("peasants_work");
                var peasantsMax = reader.GetOrdinal("peasants_max");

                while (reader.Read())
                {
                    peasants.PlayerId = reader.GetString(playerId);
                    peasants.PeasantsCount = reader.GetInt32(peasantsCount);
                    peasants.PeasantsWork = reader.GetInt32(peasantsWork);
                    peasants.PeasantsMax = reader.GetInt32(peasantsMax);
                }
            }

            command.Dispose();

            return peasants;
        }

        public Peasants UpdatePeasantsInfo(Peasants peasants, SqlConnection connection)
        {
            String peasantQuery = "UPDATE dbo.PeasantsData SET peasants_count = @peasants_count, peasants_work  = @peasants_work, peasants_max = @peasants_max WHERE player_id = @player_id ";

            var peasantCommand = new SqlCommand(peasantQuery, connection);

            peasantCommand.Parameters.AddWithValue("@peasants_count", peasants.PeasantsCount);
            peasantCommand.Parameters.AddWithValue("@peasants_work", peasants.PeasantsWork);
            peasantCommand.Parameters.AddWithValue("@peasants_max", peasants.PeasantsMax);
            peasantCommand.Parameters.AddWithValue("@player_id", peasants.PlayerId);

            peasantCommand.ExecuteNonQuery();

            peasantCommand.Dispose();

            return peasants;
        }
    }
}
