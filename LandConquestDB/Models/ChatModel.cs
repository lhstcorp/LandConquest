using LandConquestDB.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace LandConquestDB.Models
{
    public class ChatModel
    {
        //CREATE TRIGGER [dbo].[MessageTrigger] ON [dbo].[ChatMessages] AFTER INSERT AS BEGIN DELETE TOP(1) FROM ChatMessages END     
        public static List<ChatMessages> GetMessages(string currentPlayerId, string currentRoomId)
        {
            List<ChatMessages> messages;
            string query = "SELECT * FROM [LandConquestMessagingDB].[dbo].[ChatMessages]";
            List<string> PlayerId = new List<string>();
            List<string> PlayerTargetId = new List<string>();
            List<string> Message = new List<string>();
            List<DateTime> MessageSentTime = new List<DateTime>();
            List<string> PlayerRoomId = new List<string>();

            var connection = DbContext.GetTempSqlConnection();
            connection.Open();
            var command = new SqlCommand(query, connection);
            using (var reader = command.ExecuteReader())
            {
                var playerId = reader.GetOrdinal("player_id");
                var message = reader.GetOrdinal("player_message");
                var playerTargetId = reader.GetOrdinal("player_target_id");
                var messageTime = reader.GetOrdinal("message_sent_time");
                var playerRoomId = reader.GetOrdinal("room_id");

                while (reader.Read())
                {
                    PlayerId.Add(reader.GetString(playerId));
                    Message.Add(reader.GetString(message));
                    PlayerTargetId.Add(reader.GetString(playerTargetId));
                    MessageSentTime.Add(reader.GetDateTime(messageTime));
                    PlayerRoomId.Add(reader.GetString(playerRoomId));
                }
                reader.Close();
            }
            command.Dispose();

            messages = new List<ChatMessages>(Message.Count);

            int j = 0;
            for (int i = 0; i < Message.Count; i++)
            {
                if (PlayerTargetId[i] == "[all]" || PlayerTargetId[i] == currentPlayerId || PlayerId[i] == currentPlayerId && PlayerRoomId[i] == currentRoomId)
                {
                    messages.Add(new ChatMessages());
                    messages[j].PlayerId = PlayerId[i];
                    messages[j].PlayerName = PlayerModel.GetPlayerNameById(messages[j].PlayerId);
                    messages[j].PlayerTargetId = PlayerTargetId[i];
                    if (PlayerTargetId[i] == "[all]")
                    {
                        messages[j].PlayerTargetName = "[all]";
                    }
                    else
                    {
                        messages[j].PlayerTargetName = "[" + PlayerModel.GetPlayerNameById(messages[j].PlayerTargetId) + "]";
                    }
                    messages[j].PlayerMessage = Message[i];
                    messages[j].MessageSentTime = MessageSentTime[i];
                    messages[j].RoomId = PlayerRoomId[i];
                    if (PlayerRoomId[i] == "0")
                    {
                        messages[j].RoomId = "0";
                    }
                    else
                    {
                        messages[j].RoomId = "[" + PlayerModel.GetPlayerNameById(messages[j].RoomId) + "]";
                    }
                    j++;
                }
            }
            connection.Close();
            return messages;
        }

        public static void SendMessage(string message, string playerId, string playerTargetId, string currentRoomId)
        {
            string query = "INSERT INTO [LandConquestMessagingDB].[dbo].[ChatMessages] (player_id, player_message, player_target_id, message_sent_time, room_id) VALUES (@player_id, @player_message, @player_target_id, @message_sent_time, @room_id)";
            
            var userCommand = new SqlCommand(query, DbContext.GetSqlConnection());

            userCommand.Parameters.AddWithValue("@player_id", playerId);
            userCommand.Parameters.AddWithValue("@player_message", message);
            userCommand.Parameters.AddWithValue("@player_target_id", playerTargetId);
            userCommand.Parameters.AddWithValue("@message_sent_time", DateTime.UtcNow);
            userCommand.Parameters.AddWithValue("@room_id", currentRoomId);

            userCommand.ExecuteNonQuery();
            userCommand.Dispose();
        }
    }
}
