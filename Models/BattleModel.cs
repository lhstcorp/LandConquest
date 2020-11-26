using LandConquest.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandConquest.Models
{
    class BattleModel
    {
        public List<ArmyInBattle> GetArmiesInfo(SqlConnection connection, List<ArmyInBattle> armies, War war)
        {
            String armyQuery = "SELECT * FROM dbo.ArmyDataInBattle WHERE war_id = @war_id";
            List<string> armiesPlayerId = new List<string>();
            List<string> armiesArmyId = new List<string>();
            List<int> armiesSizeCurrent = new List<int>();
            List<int> armiesArmyType = new List<int>();
            List<int> armiesArmyArchersCount = new List<int>();
            List<int> armiesArmyInfantryCount = new List<int>();
            List<int> armiesArmyHorsemanCount = new List<int>();
            List<int> armiesArmySiegegunCount = new List<int>();
            List<int> armiesLocalLandId = new List<int>();
            List<int> armiesArmySide = new List<int>();

            var command = new SqlCommand(armyQuery, connection);

            command.Parameters.AddWithValue("@war_id", war.WarId);

            using (var reader = command.ExecuteReader())
            {
                var playerId = reader.GetOrdinal("player_id");
                var armyId = reader.GetOrdinal("army_id");
                var armySizeCurrent = reader.GetOrdinal("army_size_current");
                var armyType = reader.GetOrdinal("army_type");
                var armyArchersCount = reader.GetOrdinal("army_archers_count");
                var armyInfantryCount = reader.GetOrdinal("army_infantry_count");
                var armyHorsemanCount = reader.GetOrdinal("army_horseman_count");
                var armySiegegunCount = reader.GetOrdinal("army_siegegun_count");
                var armyLocalLandId = reader.GetOrdinal("local_land_id");
                var armyArmySide = reader.GetOrdinal("army_side");

                while (reader.Read())
                {
                    armiesPlayerId.Add(reader.GetString(playerId));
                    armiesArmyId.Add(reader.GetString(armyId));
                    armiesSizeCurrent.Add(reader.GetInt32(armySizeCurrent));
                    armiesArmyType.Add(reader.GetInt32(armyType));
                    armiesArmyArchersCount.Add(reader.GetInt32(armyArchersCount));
                    armiesArmyInfantryCount.Add(reader.GetInt32(armyInfantryCount));
                    armiesArmyHorsemanCount.Add(reader.GetInt32(armyHorsemanCount));
                    armiesArmySiegegunCount.Add(reader.GetInt32(armySiegegunCount));
                    armiesLocalLandId.Add(reader.GetInt32(armyLocalLandId));
                    armiesArmySide.Add(reader.GetInt32(armyArmySide));
                }
            }

            command.Dispose();

            for (int i = 0; i < armiesPlayerId.Count; i++)
            {
                armies.Add(new ArmyInBattle());
                armies[i].PlayerId = armiesPlayerId[i];
                armies[i].ArmyId = armiesArmyId[i];
                armies[i].ArmySizeCurrent = armiesSizeCurrent[i];
                armies[i].ArmyType = armiesArmyType[i];
                armies[i].ArmyArchersCount = armiesArmyArchersCount[i];
                armies[i].ArmyInfantryCount = armiesArmyInfantryCount[i];
                armies[i].ArmyHorsemanCount = armiesArmyHorsemanCount[i];
                armies[i].ArmySiegegunCount = armiesArmySiegegunCount[i];
                armies[i].LocalLandId = armiesLocalLandId[i];
                armies[i].ArmySide = armiesArmySide[i];
            }

            armiesPlayerId = null;
            armiesArmyId = null;
            armiesSizeCurrent = null;
            armiesArmyType = null;
            armiesArmyArchersCount = null;
            armiesArmyInfantryCount = null;
            armiesArmyHorsemanCount = null;
            armiesArmySiegegunCount = null;
            armiesLocalLandId = null;
            armiesArmySide = null;

            return armies;
        }

        public void InsertArmyIntoBattleTable(SqlConnection connection, ArmyInBattle army, War war)
        {
            String armyQuery = "INSERT INTO dbo.ArmyDataInBattle (player_id, army_id, army_size_current, army_type, army_archers_count, army_infantry_count, army_horseman_count, army_siegegun_count, local_land_id, army_side, war_id) VALUES (@player_id, @army_id, @army_size_current, @army_type, @army_archers_count, @army_infantry_count, @army_horseman_count, @army_siegegun_count, @local_land_id, @army_side, @war_id)";
            var armyCommand = new SqlCommand(armyQuery, connection);

            armyCommand.Parameters.AddWithValue("@player_id", army.PlayerId);
            armyCommand.Parameters.AddWithValue("@army_id", army.ArmyId);
            armyCommand.Parameters.AddWithValue("@army_size_current", army.ArmySizeCurrent);
            armyCommand.Parameters.AddWithValue("@army_type", army.ArmyType);
            armyCommand.Parameters.AddWithValue("@army_archers_count", army.ArmyArchersCount);
            armyCommand.Parameters.AddWithValue("@army_infantry_count", army.ArmyInfantryCount);
            armyCommand.Parameters.AddWithValue("@army_horseman_count", army.ArmyHorsemanCount);
            armyCommand.Parameters.AddWithValue("@army_siegegun_count", army.ArmySiegegunCount);
            armyCommand.Parameters.AddWithValue("@local_land_id", army.LocalLandId);
            armyCommand.Parameters.AddWithValue("@army_side", army.ArmySide);
            armyCommand.Parameters.AddWithValue("@war_id", war.WarId);

            armyCommand.ExecuteNonQuery();

            armyCommand.Dispose();
        }

        public int SelectLastIdOfArmies(SqlConnection connection, War war)
        {
            String Query = "SELECT * FROM dbo.ArmyDataInBattle WHERE war_id = @war_id";
            var Command = new SqlCommand(Query, connection);
            Command.Parameters.AddWithValue("@war_id", war.WarId);
            string armyId = "";
            int count = 0;
            using (var reader = Command.ExecuteReader())
            {
                var stateId = reader.GetOrdinal("army_id");
                while (reader.Read())
                {
                    armyId = reader.GetString(stateId);
                    count++;
                }
            }

            Command.Dispose();
            return count;
        }

        public int SelectLastIdOfArmiesInCurrentTile(SqlConnection connection, int index, War war)
        {
            String Query = "SELECT * FROM dbo.ArmyDataInBattle WHERE war_id = @war_id AND local_land_id = @local_land_id";
            var Command = new SqlCommand(Query, connection);

            Command.Parameters.AddWithValue("@war_id", war.WarId);
            Command.Parameters.AddWithValue("@local_land_id", index);

            string armyId = "";
            int count = 0;
            using (var reader = Command.ExecuteReader())
            {
                var stateId = reader.GetOrdinal("army_id");
                while (reader.Read())
                {
                    armyId = reader.GetString(stateId);
                    count++;
                }
            }

            Command.Dispose();
            return count;
        }

        public List<ArmyInBattle> GetArmiesInfoInCurrentTile(SqlConnection connection, List<ArmyInBattle> armies, War war, int index)
        {
            String armyQuery = "SELECT * FROM dbo.ArmyDataInBattle WHERE war_id = @war_id AND local_land_id = @local_land_id";

            List<string> armiesPlayerId = new List<string>();
            List<string> armiesArmyId = new List<string>();
            List<int> armiesSizeCurrent = new List<int>();
            List<int> armiesArmyType = new List<int>();
            List<int> armiesArmyArchersCount = new List<int>();
            List<int> armiesArmyInfantryCount = new List<int>();
            List<int> armiesArmyHorsemanCount = new List<int>();
            List<int> armiesArmySiegegunCount = new List<int>();
            List<int> armiesLocalLandId = new List<int>();
            List<int> armiesArmySide = new List<int>();

            var command = new SqlCommand(armyQuery, connection);

            command.Parameters.AddWithValue("@war_id", war.WarId);
            command.Parameters.AddWithValue("@local_land_id", index);

            using (var reader = command.ExecuteReader())
            {
                var playerId = reader.GetOrdinal("player_id");
                var armyId = reader.GetOrdinal("army_id");
                var armySizeCurrent = reader.GetOrdinal("army_size_current");
                var armyType = reader.GetOrdinal("army_type");
                var armyArchersCount = reader.GetOrdinal("army_archers_count");
                var armyInfantryCount = reader.GetOrdinal("army_infantry_count");
                var armyHorsemanCount = reader.GetOrdinal("army_horseman_count");
                var armySiegegunCount = reader.GetOrdinal("army_siegegun_count");
                var armyLocalLandId = reader.GetOrdinal("local_land_id");
                var armyArmySide = reader.GetOrdinal("army_side");

                while (reader.Read())
                {
                    armiesPlayerId.Add(reader.GetString(playerId));
                    armiesArmyId.Add(reader.GetString(armyId));
                    armiesSizeCurrent.Add(reader.GetInt32(armySizeCurrent));
                    armiesArmyType.Add(reader.GetInt32(armyType));
                    armiesArmyArchersCount.Add(reader.GetInt32(armyArchersCount));
                    armiesArmyInfantryCount.Add(reader.GetInt32(armyInfantryCount));
                    armiesArmyHorsemanCount.Add(reader.GetInt32(armyHorsemanCount));
                    armiesArmySiegegunCount.Add(reader.GetInt32(armySiegegunCount));
                    armiesLocalLandId.Add(reader.GetInt32(armyLocalLandId));
                    armiesArmySide.Add(reader.GetInt32(armyArmySide));
                }
            }

            command.Dispose();

            for (int i = 0; i < armiesPlayerId.Count; i++)
            {
                armies[i].PlayerId = armiesPlayerId[i];
                armies[i].ArmyId = armiesArmyId[i];
                armies[i].ArmySizeCurrent = armiesSizeCurrent[i];
                armies[i].ArmyType = armiesArmyType[i];
                armies[i].ArmyArchersCount = armiesArmyArchersCount[i];
                armies[i].ArmyInfantryCount = armiesArmyInfantryCount[i];
                armies[i].ArmyHorsemanCount = armiesArmyHorsemanCount[i];
                armies[i].ArmySiegegunCount = armiesArmySiegegunCount[i];
                armies[i].LocalLandId = armiesLocalLandId[i];
                armies[i].ArmySide = armiesArmySide[i];
            }

            armiesPlayerId = null;
            armiesArmyId = null;
            armiesSizeCurrent = null;
            armiesArmyType = null;
            armiesArmyArchersCount = null;
            armiesArmyInfantryCount = null;
            armiesArmyHorsemanCount = null;
            armiesArmySiegegunCount = null;
            armiesLocalLandId = null;
            armiesArmySide = null;

            return armies;
        }

        public void UpdateLocalLandOfArmy(SqlConnection connection, ArmyInBattle selectedArmy, int index)
        {
            String ArmyQuery = "UPDATE dbo.ArmyDataInBattle SET local_land_id = @local_land_id WHERE army_id = @army_id";

            var ArmyCommand = new SqlCommand(ArmyQuery, connection);
            ArmyCommand.Parameters.AddWithValue("@local_land_id", index);
            ArmyCommand.Parameters.AddWithValue("@army_id", selectedArmy.ArmyId);

            ArmyCommand.ExecuteNonQuery();

            ArmyCommand.Dispose();
        }

        public int CheckPlayerParticipation(SqlConnection connection, Player player)
        {
            String query = "SELECT * FROM dbo.ArmyDataInBattle WHERE player_id = @player_id";

            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@player_id", player.PlayerId);

            int counter = 0;

            using (var reader = command.ExecuteReader())
            {
                var playerId = reader.GetOrdinal("player_id");
                while (reader.Read())
                {
                    player.PlayerId = reader.GetString(playerId);
                    counter++;
                }
            }
            return counter;
        }

        public void UpdateArmyType(SqlConnection connection, Army army)
        {
            String storageQuery = "UPDATE dbo.ArmyDataInBattle SET army_type = @army_type WHERE army_id = @army_id";

            var storageCommand = new SqlCommand(storageQuery, connection);
            // int datetimeResult;
            storageCommand.Parameters.AddWithValue("@army_type", army.ArmyType);
            storageCommand.Parameters.AddWithValue("@army_id", army.ArmyId);

            storageCommand.ExecuteNonQuery();


            storageCommand.Dispose();
        }

        public void UpdateArmyInBattle(SqlConnection connection, Army army)
        {
            String storageQuery = "UPDATE dbo.ArmyDataInBattle SET army_size_current = @army_size_current, army_type = @army_type, army_archers_count = @army_archers_count, army_infantry_count = @army_infantry_count, army_horseman_count = @army_horseman_count, army_siegegun_count = @army_siegegun_count WHERE army_id = @army_id";

            var storageCommand = new SqlCommand(storageQuery, connection);
            // int datetimeResult;
            storageCommand.Parameters.AddWithValue("@army_size_current", army.ArmySizeCurrent);
            storageCommand.Parameters.AddWithValue("@army_type", army.ArmyType);
            storageCommand.Parameters.AddWithValue("@army_archers_count", army.ArmyArchersCount);
            storageCommand.Parameters.AddWithValue("@army_infantry_count", army.ArmyInfantryCount);
            storageCommand.Parameters.AddWithValue("@army_horseman_count", army.ArmyHorsemanCount);
            storageCommand.Parameters.AddWithValue("@army_siegegun_count", army.ArmySiegegunCount);
            storageCommand.Parameters.AddWithValue("@army_id", army.ArmyId);

            storageCommand.ExecuteNonQuery();

            storageCommand.Dispose();
        }

        public int ReturnTypeOfArmy(List<ArmyInBattle> armies)
        {
            for (int i = 0; i < armies.Count - 1; i++)
            {
                if (armies[i].ArmyType == armies[i + 1].ArmyType) continue;
                else return 5;
            }

            return armies[0].ArmyType;
        }

        public void DeleteArmyById(SqlConnection connection, ArmyInBattle army)
        {
            String query = "DELETE FROM dbo.ArmyDataInBattle WHERE army_id = @army_id";

            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@army_id", army.ArmyId);

            command.ExecuteNonQuery();

            command.Dispose();
        }

        public int ReturnTypeOfUnionArmy(ArmyInBattle army)
        {
            if ((army.ArmyInfantryCount > 0) && (army.ArmyInfantryCount == army.ArmySizeCurrent))
                return 1;
            else
            if ((army.ArmyArchersCount > 0) && (army.ArmyArchersCount == army.ArmySizeCurrent))
                return 2;
            else
            if ((army.ArmyHorsemanCount > 0) && (army.ArmyHorsemanCount == army.ArmySizeCurrent))
                return 3;
            else
            if ((army.ArmySiegegunCount > 0) && (army.ArmySiegegunCount == army.ArmySizeCurrent))
                return 4;
            return 5;
        }

        public bool IfTheBattleShouldStart(List<ArmyInBattle> armies)
        {
            for (int i = 0; i < armies.Count; i++)
            {
                if (armies[0].ArmySide != armies[i].ArmySide) return true;
            }
            return false;
        }

        public void InsertBattle(SqlConnection connection, Battle battle)
        {
            String battleQuery = "INSERT INTO dbo.BattleData (battle_id, war_id, local_land_id) VALUES (@battle_id, @war_id, @local_land_id)";
            var battleCommand = new SqlCommand(battleQuery, connection);

            battleCommand.Parameters.AddWithValue("@battle_id", battle.BattleId);
            battleCommand.Parameters.AddWithValue("@war_id", battle.WarId);
            battleCommand.Parameters.AddWithValue("@local_land_id", battle.LocalLandId);

            battleCommand.ExecuteNonQuery();

            battleCommand.Dispose();
        }


        public bool DidTheWarStarted(SqlConnection connection, int index, War war)
        {
            String Query = "SELECT * FROM dbo.BattleData WHERE war_id = @war_id AND local_land_id = @local_land_id";
            var Command = new SqlCommand(Query, connection);

            Command.Parameters.AddWithValue("@war_id", war.WarId);
            Command.Parameters.AddWithValue("@local_land_id", index);

            //string armyId = "";
            int count = 0;
            using (var reader = Command.ExecuteReader())
            {
                //var stateId = reader.GetOrdinal("war_id");
                while (reader.Read())
                {
                    //armyId = reader.GetString(war);
                    count++;
                }
            }

            if (count == 0)
                return false;

            return true;
        }

        public List<ArmyInBattle> GetPlayerArmiesInfo(SqlConnection connection, List<ArmyInBattle> armies, War war, Player player)
        {
            String armyQuery = "SELECT * FROM dbo.ArmyDataInBattle WHERE war_id = @war_id AND player_id = @player_id";
            List<string> armiesPlayerId = new List<string>();
            List<string> armiesArmyId = new List<string>();
            List<int> armiesSizeCurrent = new List<int>();
            List<int> armiesArmyType = new List<int>();
            List<int> armiesArmyArchersCount = new List<int>();
            List<int> armiesArmyInfantryCount = new List<int>();
            List<int> armiesArmyHorsemanCount = new List<int>();
            List<int> armiesArmySiegegunCount = new List<int>();
            List<int> armiesLocalLandId = new List<int>();
            List<int> armiesArmySide = new List<int>();

            var command = new SqlCommand(armyQuery, connection);

            command.Parameters.AddWithValue("@war_id", war.WarId);
            command.Parameters.AddWithValue("@player_id", player.PlayerId);

            using (var reader = command.ExecuteReader())
            {
                var playerId = reader.GetOrdinal("player_id");
                var armyId = reader.GetOrdinal("army_id");
                var armySizeCurrent = reader.GetOrdinal("army_size_current");
                var armyType = reader.GetOrdinal("army_type");
                var armyArchersCount = reader.GetOrdinal("army_archers_count");
                var armyInfantryCount = reader.GetOrdinal("army_infantry_count");
                var armyHorsemanCount = reader.GetOrdinal("army_horseman_count");
                var armySiegegunCount = reader.GetOrdinal("army_siegegun_count");
                var armyLocalLandId = reader.GetOrdinal("local_land_id");
                var armyArmySide = reader.GetOrdinal("army_side");

                while (reader.Read())
                {
                    armiesPlayerId.Add(reader.GetString(playerId));
                    armiesArmyId.Add(reader.GetString(armyId));
                    armiesSizeCurrent.Add(reader.GetInt32(armySizeCurrent));
                    armiesArmyType.Add(reader.GetInt32(armyType));
                    armiesArmyArchersCount.Add(reader.GetInt32(armyArchersCount));
                    armiesArmyInfantryCount.Add(reader.GetInt32(armyInfantryCount));
                    armiesArmyHorsemanCount.Add(reader.GetInt32(armyHorsemanCount));
                    armiesArmySiegegunCount.Add(reader.GetInt32(armySiegegunCount));
                    armiesLocalLandId.Add(reader.GetInt32(armyLocalLandId));
                    armiesArmySide.Add(reader.GetInt32(armyArmySide));
                }
            }

            command.Dispose();

            for (int i = 0; i < armiesPlayerId.Count; i++)
            {
                armies.Add(new ArmyInBattle());
                armies[i].PlayerId = armiesPlayerId[i];
                armies[i].ArmyId = armiesArmyId[i];
                armies[i].ArmySizeCurrent = armiesSizeCurrent[i];
                armies[i].ArmyType = armiesArmyType[i];
                armies[i].ArmyArchersCount = armiesArmyArchersCount[i];
                armies[i].ArmyInfantryCount = armiesArmyInfantryCount[i];
                armies[i].ArmyHorsemanCount = armiesArmyHorsemanCount[i];
                armies[i].ArmySiegegunCount = armiesArmySiegegunCount[i];
                armies[i].LocalLandId = armiesLocalLandId[i];
                armies[i].ArmySide = armiesArmySide[i];
            }

            armiesPlayerId = null;
            armiesArmyId = null;
            armiesSizeCurrent = null;
            armiesArmyType = null;
            armiesArmyArchersCount = null;
            armiesArmyInfantryCount = null;
            armiesArmyHorsemanCount = null;
            armiesArmySiegegunCount = null;
            armiesLocalLandId = null;
            armiesArmySide = null;

            return armies;
        }
    }
}
