using LandConquestDB.Entities;
using System.Data.SqlClient;

namespace LandConquestDB.Models
{
    public class PeasantModel
    {
        public static Peasants GetPeasantsInfo(Player player, Peasants peasants)
        {
            string query = "SELECT * FROM dbo.PeasantsData WHERE player_id = @player_id";

            var command = new SqlCommand(query, DbContext.GetSqlConnection());
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
                reader.Close();
            }
            command.Dispose();

            return peasants;
        }

        public static Peasants UpdatePeasantsInfo(Peasants peasants)
        {
            string peasantQuery = "UPDATE dbo.PeasantsData SET peasants_count = @peasants_count, peasants_work  = @peasants_work, peasants_max = @peasants_max WHERE player_id = @player_id ";

            var peasantCommand = new SqlCommand(peasantQuery, DbContext.GetSqlConnection());

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
