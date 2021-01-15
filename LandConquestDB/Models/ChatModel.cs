using LandConquestDB.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace LandConquestDB.Models
{
    public class ChatModel
    {
        //CREATE TRIGGER [dbo].[MessageTrigger] ON [dbo].[ChatMessages] AFTER INSERT AS BEGIN DELETE TOP(1) FROM ChatMessages END     
        public static List<ChatMessages> GetMessages()
        {
            List<ChatMessages> messages;
            String query = "SELECT * FROM dbo.ChatMessages";
            List<string> PlayerName = new List<string>();
            List<string> Message = new List<string>();
            List<DateTime> MessageTime = new List<DateTime>();

            var command = new SqlCommand(query, DbContext.GetConnection());
            using (var reader = command.ExecuteReader())
            {
                var playerName = reader.GetOrdinal("player_name");
                var manufactureId = reader.GetOrdinal("player_message");
                var messageTime = reader.GetOrdinal("message_sent_time");

                while (reader.Read())
                {
                    PlayerName.Add(reader.GetString(playerName));
                    Message.Add(reader.GetString(manufactureId));
                    MessageTime.Add(reader.GetDateTime(messageTime));

                }
            }
            command.Dispose();

            messages = new List<ChatMessages>();
            for (int i = 0; i < Message.Count; i++)
            {
                messages.Add(new ChatMessages());
            }

            for (int i = 0; i < Message.Count; i++)
            {
                messages[i].PlayerName = PlayerName[i];
                messages[i].PlayerMessage = Message[i];
                messages[i].MessageTime = MessageTime[i];
            }
            return messages;
        }

        public static void SendMessage(string message, string playerName)
        {
            String query = "INSERT INTO dbo.ChatMessages (player_name, player_message, message_sent_time) VALUES (@player_name, @player_message, @message_sent_time)";
            var userCommand = new SqlCommand(query, DbContext.GetConnection());

            userCommand.Parameters.AddWithValue("@player_name", playerName);
            userCommand.Parameters.AddWithValue("@player_message", message);
            userCommand.Parameters.AddWithValue("@message_sent_time", DateTime.UtcNow);

            userCommand.ExecuteNonQuery();
        }

        public static void EnableBroker()
        {
            string query = "ALTER DATABASE LandConquestDBDB SET ENABLE_BROKER with rollback immediate";
            var peasantCommand = new SqlCommand(query, DbContext.GetConnection());
            peasantCommand.ExecuteNonQuery();
        }
    }
}
