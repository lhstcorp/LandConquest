using LandConquestDB.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace LandConquestDB.Models
{
    public class ChatModel
    {
        //CREATE TRIGGER [dbo].[MessageTrigger] ON [dbo].[ChatMessages] AFTER INSERT AS BEGIN DELETE TOP(1) FROM ChatMessages END     
        public static List<ChatMessages> GetMessages(string currentPlayerId)
        {
            List<ChatMessages> messages;
            string query = "SELECT * FROM [LandConquestMessagingDB].[dbo].[ChatMessages]";
            List<string> PlayerId = new List<string>();
            List<string> PlayerTargerId = new List<string>();
            List<string> Message = new List<string>();
            List<DateTime> MessageSentTime = new List<DateTime>();

            var connection = DbContext.GetTempSqlConnection();
            connection.Open();
            var command = new SqlCommand(query, connection);
            using (var reader = command.ExecuteReader())
            {
                var playerId = reader.GetOrdinal("player_id");
                var message = reader.GetOrdinal("player_message");
                var playerTargetId = reader.GetOrdinal("player_target_id");
                var messageTime = reader.GetOrdinal("message_sent_time");

                while (reader.Read())
                {
                    PlayerId.Add(reader.GetString(playerId));
                    Message.Add(reader.GetString(message));
                    PlayerTargerId.Add(reader.GetString(playerTargetId));
                    MessageSentTime.Add(reader.GetDateTime(messageTime));
                }
                reader.Close();
            }
            command.Dispose();

            messages = new List<ChatMessages>(Message.Count);

            for (int i = 0; i < Message.Count; i++)
            {
                if (PlayerTargerId[i] == "[all]" || PlayerTargerId[i] == currentPlayerId)
                {
                    messages.Add(new ChatMessages());
                    messages[i].PlayerId = PlayerId[i];
                    messages[i].PlayerName = PlayerModel.GetPlayerNameById(messages[i].PlayerId);
                    messages[i].PlayerTargetId = PlayerTargerId[i];
                    if (PlayerTargerId[i] == "[all]")
                    {
                        messages[i].PlayerTargetName = "[all]";
                    }
                    else
                    {
                        messages[i].PlayerTargetName = PlayerModel.GetPlayerNameById(messages[i].PlayerTargetId);
                    }
                    messages[i].PlayerMessage = Message[i];
                    messages[i].MessageSentTime = MessageSentTime[i];
                }
            }
            connection.Close();
            return messages;
        }

        public static void SendMessage(string message, string playerId, string playerTargetId)
        {
            string query = "INSERT INTO [LandConquestMessagingDB].[dbo].[ChatMessages] (player_id, player_message, player_target_id, message_sent_time) VALUES (@player_id, @player_message, @player_target_id, @message_sent_time)";
            
            var userCommand = new SqlCommand(query, DbContext.GetSqlConnection());

            userCommand.Parameters.AddWithValue("@player_id", playerId);
            userCommand.Parameters.AddWithValue("@player_message", message);
            userCommand.Parameters.AddWithValue("@player_target_id", playerTargetId);
            userCommand.Parameters.AddWithValue("@message_sent_time", DateTime.UtcNow);

            userCommand.ExecuteNonQuery();
            userCommand.Dispose();
        }
    }
}
