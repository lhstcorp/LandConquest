using LandConquest.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandConquest.Models
{
    public class ChatModel
    {
        //CREATE TRIGGER [dbo].[MessageTrigger] ON [dbo].[ChatMessages] AFTER INSERT AS BEGIN DELETE TOP(1) FROM ChatMessages END

        List<ChatMessages> messages;
        public List<ChatMessages> GetMessages(SqlConnection connection)
        {

            String query = "SELECT * FROM dbo.ChatMessages";
            List<string> PlayerName = new List<string>();
            List<string> Message = new List<string>();
            List<DateTime> MessageTime = new List<DateTime>();

            var command = new SqlCommand(query, connection);
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

        public void SendMessage(string message, SqlConnection connection, string playerName)
        {
            String query = "INSERT INTO dbo.ChatMessages (player_name, player_message, message_sent_time) VALUES (@player_name, @player_message, @message_sent_time)";
            var userCommand = new SqlCommand(query, connection);

            userCommand.Parameters.AddWithValue("@player_name", playerName);
            userCommand.Parameters.AddWithValue("@player_message", message);
            userCommand.Parameters.AddWithValue("@message_sent_time", DateTime.UtcNow);

            userCommand.ExecuteNonQuery();
        }

        public static void EnableBroker(SqlConnection connection)
        {
            string query = "ALTER DATABASE LandConquestDB SET ENABLE_BROKER with rollback immediate";
            var peasantCommand = new SqlCommand(query, connection);
            peasantCommand.ExecuteNonQuery();
        }
    }
}
