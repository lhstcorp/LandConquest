using LandConquestDB.Entities;
using System;
using System.Data.SqlClient;

namespace LandConquestDB.Models
{
    public class PlayerEntranceModel
    {
        public static DateTime GetLastEntrance(Player player)
        { 
            string query = "SELECT * FROM dbo.PlayerEntranceData WHERE player_id = @player_id";
            DateTime dateTime = new DateTime();

            var command = new SqlCommand(query, DbContext.GetSqlConnection());
            command.Parameters.AddWithValue("@player_id", player.PlayerId);

            using (var reader = command.ExecuteReader())
            {
                var playerId = reader.GetOrdinal("last_entrance");


                while (reader.Read())
                {
                    dateTime = reader.GetDateTime(playerId);
                }
                return dateTime;
            }  
        }

        public static DateTime GetFirstEntrance(Player player)
        {
            string query = "SELECT * FROM dbo.PlayerEntranceData WHERE player_id = @player_id";
            DateTime dateTime = new DateTime();

            var command = new SqlCommand(query, DbContext.GetSqlConnection());
            command.Parameters.AddWithValue("@player_id", player.PlayerId);

            using (var reader = command.ExecuteReader())
            {
                var playerId = reader.GetOrdinal("first_entrance");


                while (reader.Read())
                {
                    dateTime = reader.GetDateTime(playerId);
                }
                return dateTime;
            }
        }

        public static void SetFirstEntrance(Player player)
        {
            string query = "INSERT INTO dbo.PlayerEntranceData (player_id, last_entrance, first_entrance) VALUES (@player_id, @last_entrance, @first_entrance)";
            var command = new SqlCommand(query, DbContext.GetSqlConnection());

            command.Parameters.AddWithValue("@player_id", player.PlayerId);
            command.Parameters.AddWithValue("@last_entrance", DateTime.UtcNow);
            command.Parameters.AddWithValue("@first_entrance", DateTime.UtcNow);

            command.ExecuteNonQuery();
            command.Dispose();
        }

        public static void UpdateLastEntrance(Player player)
        {
            string query = "UPDATE dbo.PlayerEntranceData SET last_entrance = @last_entrance WHERE player_id = @player_id ";

            var command = new SqlCommand(query, DbContext.GetSqlConnection());
            command.Parameters.AddWithValue("@last_entrance", DateTime.UtcNow);
            command.Parameters.AddWithValue("@player_id", player.PlayerId);

            command.ExecuteNonQuery();
            command.Dispose();
        }
    }
}
