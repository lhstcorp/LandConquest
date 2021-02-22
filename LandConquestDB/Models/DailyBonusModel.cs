using LandConquestDB.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace LandConquestDB.Models
{
    public class DailyBonusModel
    {
        public static DateTime GetNextDailyBonusTime(Player player)
        {
            string query = "SELECT * FROM dbo.DailyBonus WHERE player_id = @player_id";
            DateTime dateTime = new DateTime();

            var command = new SqlCommand(query, DbContext.GetSqlConnection());
            command.Parameters.AddWithValue("@player_id", player.PlayerId);

            using (var reader = command.ExecuteReader())
            {
                var playerId = reader.GetOrdinal("next_daily_bonus");


                while (reader.Read())
                {
                    dateTime = reader.GetDateTime(playerId);
                }
                reader.Close();
            }
            command.Dispose();

            return dateTime;
        }
        public static void SetFirstDailyBonusTime(Player player)
        {
            string query = "INSERT INTO dbo.DailyBonus (player_id, next_daily_bonus) VALUES (@player_id, @next_daily_bonus)";
            var command = new SqlCommand(query, DbContext.GetSqlConnection());

            command.Parameters.AddWithValue("@player_id", player.PlayerId);
            command.Parameters.AddWithValue("@next_daily_bonus", DateTime.UtcNow);

            command.ExecuteNonQuery();
            command.Dispose();
        }

        public static void UpdateNextDailyBonusTime(Player player)
        {
            string query = "UPDATE dbo.DailyBonus SET next_daily_bonus = @next_daily_bonus WHERE player_id = @player_id ";

            var todayDate = DateTime.UtcNow;

            var command = new SqlCommand(query, DbContext.GetSqlConnection());
            command.Parameters.AddWithValue("@next_daily_bonus", todayDate.AddDays(1));
            command.Parameters.AddWithValue("@player_id", player.PlayerId);

            command.ExecuteNonQuery();
            command.Dispose();
        }

        public static void GiveDailyBonusReward(string playerId, string resource, int amount)
        {
            string storageQuery = "UPDATE dbo.StorageData SET " + resource + " = " + resource + " + @amount WHERE player_id = @player_id ";

            var storageCommand = new SqlCommand(storageQuery, DbContext.GetSqlConnection());

            storageCommand.Parameters.AddWithValue("@player_id", playerId);
            storageCommand.Parameters.AddWithValue("@amount", amount);
            storageCommand.ExecuteNonQuery();
            storageCommand.Dispose();
        }
    }
}
