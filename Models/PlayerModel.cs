using LandConquest.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandConquest.Models
{
    public class PlayerModel
    {
        public int CreatePlayer(AuthorisationWindow window, SqlConnection connection, string userId, User registeredUser)
        {
            registeredUser.UserId = userId;
            registeredUser.UserLogin = window.textBoxNewLogin.Text;
            registeredUser.UserEmail = window.textBoxNewEmail.Text;
            registeredUser.UserPass = window.textBoxNewPass.Text;

            String playerQuery = "INSERT INTO dbo.PlayerData (player_id,player_name,player_exp,player_lvl, player_money, player_donation, player_title, player_current_region) " +
                "VALUES (@player_id,@player_name,@player_exp,@player_lvl, @player_money, @player_donation, @player_title, @player_current_region)";
            var playerCommand = new SqlCommand(playerQuery, connection);

            playerCommand.Parameters.AddWithValue("@player_id", registeredUser.UserId);
            playerCommand.Parameters.AddWithValue("@player_name", registeredUser.UserLogin);
            playerCommand.Parameters.AddWithValue("@player_exp", 0);
            playerCommand.Parameters.AddWithValue("@player_lvl", 1);
            playerCommand.Parameters.AddWithValue("@player_money", 1);
            playerCommand.Parameters.AddWithValue("@player_donation", 2);

            //playerCommand.Parameters.AddWithValue("@player_image", image);
            playerCommand.Parameters.AddWithValue("@player_title", 1);

            playerCommand.Parameters.AddWithValue("@player_current_region", 1);

            int playerResult = playerCommand.ExecuteNonQuery();

            return playerResult;
        }

        public void CreatePlayerResources(AuthorisationWindow window, SqlConnection connection, string userId, User registeredUser)
        {
            // create storage for new player
            String storageQuery = "INSERT INTO dbo.StorageData (player_id) VALUES (@player_id)";
            var storageCommand = new SqlCommand(storageQuery, connection);


            storageCommand.Parameters.AddWithValue("@player_id", userId);

            storageCommand.ExecuteNonQuery();

            String equipmentQuery = "INSERT INTO dbo.PlayerEquipment (player_id) VALUES (@player_id)";
            var equipmentCommand = new SqlCommand(equipmentQuery, connection);


            equipmentCommand.Parameters.AddWithValue("@player_id", userId);

            equipmentCommand.ExecuteNonQuery();

            // create default manufactures for new player
            String manufactureQuery = "INSERT INTO dbo.ManufactureData (player_id,manufacture_id,manufacture_name,manufacture_type) VALUES (@player_id, @manufacture_id, @manufacture_name, @manufacture_type)";

            //wood
            var woodCommand = new SqlCommand(manufactureQuery, connection);


            woodCommand.Parameters.AddWithValue("@player_id", userId);
            woodCommand.Parameters.AddWithValue("@manufacture_id", AuthorisationWindow.generateUserId());
            woodCommand.Parameters.AddWithValue("@manufacture_name", "Sawmill");
            woodCommand.Parameters.AddWithValue("@manufacture_type", 1);

            woodCommand.ExecuteNonQuery();

            //stone
            var stoneCommand = new SqlCommand(manufactureQuery, connection);

            stoneCommand.Parameters.AddWithValue("@player_id", userId);
            stoneCommand.Parameters.AddWithValue("@manufacture_id", AuthorisationWindow.generateUserId());
            stoneCommand.Parameters.AddWithValue("@manufacture_name", "Quarry");
            stoneCommand.Parameters.AddWithValue("@manufacture_type", 2);

            stoneCommand.ExecuteNonQuery();

            //food
            var foodCommand = new SqlCommand(manufactureQuery, connection);
            foodCommand.Parameters.AddWithValue("@player_id", userId);
            foodCommand.Parameters.AddWithValue("@manufacture_id", AuthorisationWindow.generateUserId());
            foodCommand.Parameters.AddWithValue("@manufacture_name", "Windmill");
            foodCommand.Parameters.AddWithValue("@manufacture_type", 3);

            foodCommand.ExecuteNonQuery();

            // create peasants data for player

            String peasantsQuery = "INSERT INTO dbo.PeasantsData (player_id) VALUES (@player_id)";
            var peasantsCommand = new SqlCommand(peasantsQuery, connection);

            peasantsCommand.Parameters.AddWithValue("@player_id", userId);

            peasantsCommand.ExecuteNonQuery();
        }

        public Player GetPlayerInfo(User user, SqlConnection connection, Player player)
        {
            String query = "SELECT * FROM dbo.PlayerData WHERE player_id = @player_id";

            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@player_id", user.UserId);

            using (var reader = command.ExecuteReader())
            {
                var playerId = reader.GetOrdinal("player_id");
                var playerName = reader.GetOrdinal("player_name");
                var playerExp = reader.GetOrdinal("player_exp");
                var playerLvl = reader.GetOrdinal("player_lvl");
                var playerMoney = reader.GetOrdinal("player_money");
                var playerDonation = reader.GetOrdinal("player_donation");
                var playerImage = reader.GetOrdinal("player_image");
                var playerTitle = reader.GetOrdinal("player_title");
                var playerCurrentRegion = reader.GetOrdinal("player_current_region");

                while (reader.Read())
                {
                    player.PlayerId = reader.GetString(playerId);
                    player.PlayerName = reader.GetString(playerName);
                    player.PlayerExp = reader.GetInt64(playerExp);
                    player.PlayerLvl = reader.GetInt32(playerLvl);
                    player.PlayerMoney = reader.GetInt64(playerMoney);
                    player.PlayerDonation = reader.GetInt64(playerDonation);
                    // player.PlayerImage = reader.GetSqlBytes(playerId);
                    player.PlayerImage = null;
                    player.PlayerTitle = reader.GetInt32(playerTitle);
                    player.PlayerCurrentRegion = reader.GetInt32(playerCurrentRegion);
                }
            }

            //command.Dispose();

            return player;
        }


        public Player UpdatePlayerMoney(Player player, SqlConnection connection)
        {
            String taxesQuery = "UPDATE dbo.PlayerData SET player_money = @player_money WHERE player_id = @player_id ";

            var taxesCommand = new SqlCommand(taxesQuery, connection);
            taxesCommand.Parameters.AddWithValue("@player_money", player.PlayerMoney);
            taxesCommand.Parameters.AddWithValue("@player_id", player.PlayerId);

            taxesCommand.ExecuteNonQuery();

            taxesCommand.Dispose();

            return player;
        }

        public Player UpdatePlayerLand(Player player, SqlConnection connection, Land land)
        {
            String taxesQuery = "UPDATE dbo.PlayerData SET player_current_region = @player_current_region WHERE player_id = @player_id ";

            var taxesCommand = new SqlCommand(taxesQuery, connection);
            taxesCommand.Parameters.AddWithValue("@player_current_region", land.LandId);
            taxesCommand.Parameters.AddWithValue("@player_id", player.PlayerId);

            player.PlayerCurrentRegion = land.LandId;

            taxesCommand.ExecuteNonQuery();

            taxesCommand.Dispose();

            return player;
        }

        public void UpdatePlayerName(SqlConnection connection, string playerId, string newPlayerName)
        {
            String userQuery = "UPDATE dbo.PlayerData SET player_name = @player_name WHERE player_id = @player_id";
            var userCommand = new SqlCommand(userQuery, connection);


            userCommand.Parameters.AddWithValue("@player_id", playerId);
            userCommand.Parameters.AddWithValue("@player_name", newPlayerName);

            userCommand.ExecuteNonQuery();
        }

        
        public List<Player> GetXpInfo(List<Player> players, SqlConnection connection, User user)
        {
            String query = "SELECT * FROM dbo.PlayerData ORDER BY player_exp desc";

            var command = new SqlCommand(query, connection);
           
            using (var reader = command.ExecuteReader())
            {

                var playerId = reader.GetOrdinal("player_id");
                var playerExp = reader.GetOrdinal("player_exp");
                var playerName = reader.GetOrdinal("player_name");

                while (reader.Read())
                {
                    Player player = new Player();
                    player.PlayerId = reader.GetString(playerId);
                    player.PlayerExp = reader.GetInt64(playerExp);
                    player.PlayerName = reader.GetString(playerName);
                    players.Add(player);
                }
            }

            return players;
        }

        public List<Player> GetCoinsInfo(List<Player> players, SqlConnection connection, User user)
        {
            String query = "SELECT * FROM dbo.PlayerData ORDER BY player_money desc";

            var command = new SqlCommand(query, connection);

            using (var reader = command.ExecuteReader())
            {

                var playerId = reader.GetOrdinal("player_id");
                var playerMoney = reader.GetOrdinal("player_money");
                var playerName = reader.GetOrdinal("player_name");

                while (reader.Read())
                {
                    Player player = new Player();
                    player.PlayerId = reader.GetString(playerId);
                    player.PlayerMoney = reader.GetInt64(playerMoney);
                    player.PlayerName = reader.GetString(playerName);
                    players.Add(player);
                }
            }

            return players;
        }

        public void UpdatePlayerExpAndLvl(Player player, SqlConnection connection)
        {
            String Query = "UPDATE dbo.PlayerData SET player_exp = @player_exp, player_lvl = @player_lvl WHERE player_id = @player_id ";

            var Command = new SqlCommand(Query, connection);
            Command.Parameters.AddWithValue("@player_exp", player.PlayerExp);
            Command.Parameters.AddWithValue("@player_lvl", player.PlayerLvl);
            Command.Parameters.AddWithValue("@player_id", player.PlayerId);

            Command.ExecuteNonQuery();

            Command.Dispose();
        }

        public List<int> DeletePlayerManufactureLandData(Peasants peasants, Player player, SqlConnection connection)
        {
            String Query = "SELECT manufacture_peasant_work FROM dbo.PlayerLandManufactureData WHERE player_id = @player_id";

            List<Int32> listPeasantsWork = new List<Int32>();

            var command = new SqlCommand(Query, connection);
            command.Parameters.AddWithValue("@player_id", player.PlayerId);

            using (var reader = command.ExecuteReader())
            {
                
                var manufacturePeasantsWork = reader.GetOrdinal("manufacture_peasant_work");

                while (reader.Read())
                {
                    listPeasantsWork.Add(reader.GetInt32(manufacturePeasantsWork));
                }


            }

            command.Dispose();

            List<int> list = new List<int>();
            if (listPeasantsWork.Count == 2)
            {
                for (int i = 0; i < listPeasantsWork.Count; i++)
                {
                    list.Add(new int());
                }

                for (int i = 0; i < listPeasantsWork.Count; i++)
                {
                    list[i] = listPeasantsWork[i];
                    peasants.PeasantsWork -= listPeasantsWork[i];
                }
            } else
            {
                for (int i = 0; i < 2; i++)
                {
                    list.Add(0);
                }
            }


            String QueryDelete = "DELETE FROM dbo.PlayerLandManufactureData WHERE player_id = @player_id ";

            var Command = new SqlCommand(QueryDelete, connection);

            Command.Parameters.AddWithValue("@player_id", player.PlayerId);

            Command.ExecuteNonQuery();

            Command.Dispose();

            //String QueryLandManufactures = "UPDATE dbo.LandManufacturesData SET manufacture_peasant_work = @manufacture_peasant_work WHERE land_id = @land_id ";

            //var CommandLandManufactures = new SqlCommand(QueryLandManufactures, connection);

            //for (int i = 0; i < 2; i++)
            //{
            //    CommandLandManufactures.Parameters.AddWithValue("@manufacture_peasant_work", );
            //    CommandLandManufactures.Parameters.AddWithValue("@player_id", player.PlayerId);

            //    CommandLandManufactures.ExecuteNonQuery();
            //}
            //CommandLandManufactures.Dispose();
            Console.WriteLine(peasants.PeasantsWork);

            return list;
        }

        public Player UpdatePlayerDonationMoney(Player player, SqlConnection connection)
        {
            String taxesQuery = "UPDATE dbo.PlayerData SET player_donation = @player_donation WHERE player_id = @player_id ";

            var taxesCommand = new SqlCommand(taxesQuery, connection);
            taxesCommand.Parameters.AddWithValue("@player_donation", player.PlayerDonation);
            taxesCommand.Parameters.AddWithValue("@player_id", player.PlayerId);

            taxesCommand.ExecuteNonQuery();

            taxesCommand.Dispose();

            return player;
        }
    }
}
