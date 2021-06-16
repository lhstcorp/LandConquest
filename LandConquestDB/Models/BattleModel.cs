using LandConquestDB.Entities;
using LandConquestDB.Forces;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace LandConquestDB.Models
{
    public class BattleModel
    {
        public static List<ArmyInBattle> GetArmiesInfo(List<ArmyInBattle> armies, War war)
        {
            string armyQuery = "SELECT * FROM dbo.ArmyDataInBattle WHERE war_id = @war_id";
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

            var command = new SqlCommand(armyQuery, DbContext.GetSqlConnection());

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
                reader.Close();
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

        public static void InsertArmyIntoBattleTable(ArmyInBattle army, War war)
        {
            string armyQuery = "INSERT INTO dbo.ArmyDataInBattle (player_id, army_id, army_size_current, army_type, army_archers_count, army_infantry_count, army_horseman_count, army_siegegun_count, local_land_id, army_side, war_id) VALUES (@player_id, @army_id, @army_size_current, @army_type, @army_archers_count, @army_infantry_count, @army_horseman_count, @army_siegegun_count, @local_land_id, @army_side, @war_id)";
            var armyCommand = new SqlCommand(armyQuery, DbContext.GetSqlConnection());

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

        public static int SelectLastIdOfArmies(War war)
        {
            string Query = "SELECT * FROM dbo.ArmyDataInBattle WHERE war_id = @war_id";
            var Command = new SqlCommand(Query, DbContext.GetSqlConnection());
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
                reader.Close();
            }

            Command.Dispose();
            return count;
        }

        public static List<ArmyInBattle> GetArmiesInfoInCurrentTile(List<ArmyInBattle> armies, War war, int index)
        {
            string armyQuery = "SELECT * FROM dbo.ArmyDataInBattle WHERE war_id = @war_id AND local_land_id = @local_land_id";

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

            var command = new SqlCommand(armyQuery, DbContext.GetSqlConnection());

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
                reader.Close();
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

        public static void UpdateLocalLandOfArmy(ArmyInBattle selectedArmy, int index)
        {
            string ArmyQuery = "UPDATE dbo.ArmyDataInBattle SET local_land_id = @local_land_id WHERE army_id = @army_id";

            var ArmyCommand = new SqlCommand(ArmyQuery, DbContext.GetSqlConnection());
            ArmyCommand.Parameters.AddWithValue("@local_land_id", index);
            ArmyCommand.Parameters.AddWithValue("@army_id", selectedArmy.ArmyId);

            ArmyCommand.ExecuteNonQuery();

            ArmyCommand.Dispose();
        }

        public static int CheckPlayerParticipation(Player player, War war)
        {
            string query = "SELECT * FROM dbo.ArmyDataInBattle WHERE player_id = @player_id AND war_id = @war_id";

            var command = new SqlCommand(query, DbContext.GetSqlConnection());
            command.Parameters.AddWithValue("@player_id", player.PlayerId);
            command.Parameters.AddWithValue("@war_id", war.WarId);

            int counter = 0;

            using (var reader = command.ExecuteReader())
            {
                var playerId = reader.GetOrdinal("player_id");
                while (reader.Read())
                {
                    player.PlayerId = reader.GetString(playerId);
                    counter++;
                }
                reader.Close();
            }
            command.Dispose();
            return counter;
        }

        public static void UpdateArmyType(Army army)
        {
            string storageQuery = "UPDATE dbo.ArmyDataInBattle SET army_type = @army_type WHERE army_id = @army_id";

            var storageCommand = new SqlCommand(storageQuery, DbContext.GetSqlConnection());
            // int datetimeResult;
            storageCommand.Parameters.AddWithValue("@army_type", army.ArmyType);
            storageCommand.Parameters.AddWithValue("@army_id", army.ArmyId);

            storageCommand.ExecuteNonQuery();
            storageCommand.Dispose();
        }

        public static void UpdateArmyInBattle(Army army)
        {
            string storageQuery = "UPDATE dbo.ArmyDataInBattle SET army_size_current = @army_size_current, army_type = @army_type, army_archers_count = @army_archers_count, army_infantry_count = @army_infantry_count, army_horseman_count = @army_horseman_count, army_siegegun_count = @army_siegegun_count WHERE army_id = @army_id";

            var storageCommand = new SqlCommand(storageQuery, DbContext.GetSqlConnection());
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


        public static void UpdateAllPlayerArmyInBattle(List<ArmyInBattle> armiesInBatle)
        {
            foreach (var army in armiesInBatle)
            {
                string storageQuery = "UPDATE dbo.ArmyDataInBattle SET army_size_current = @army_size_current, army_type = @army_type, army_archers_count = @army_archers_count, army_infantry_count = @army_infantry_count, army_horseman_count = @army_horseman_count, army_siegegun_count = @army_siegegun_count WHERE army_id = @army_id";

                var storageCommand = new SqlCommand(storageQuery, DbContext.GetSqlConnection());
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

        }

        public static int ReturnTypeOfArmy(List<ArmyInBattle> armies)
        {
            for (int i = 0; i < armies.Count - 1; i++)
            {
                if (armies[i].ArmyType == armies[i + 1].ArmyType) continue;
                else return 5;
            }

            return armies[0].ArmyType;
        }

        public static void DeleteArmyById(ArmyInBattle army)
        {
            string query = "DELETE FROM dbo.ArmyDataInBattle WHERE army_id = @army_id";

            var command = new SqlCommand(query, DbContext.GetSqlConnection());
            command.Parameters.AddWithValue("@army_id", army.ArmyId);

            command.ExecuteNonQuery();

            command.Dispose();
        }

        public static int ReturnTypeOfUnionArmy(ArmyInBattle army)
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

        public static bool IfTheBattleShouldStart(List<ArmyInBattle> armies)
        {
            for (int i = 0; i < armies.Count; i++)
            {
                if (armies[0].ArmySide != armies[i].ArmySide) return true;
            }
            return false;
        }

        public static void InsertBattle(Battle battle)
        {
            string battleQuery = "INSERT INTO dbo.BattleData (battle_id, war_id, local_land_id) VALUES (@battle_id, @war_id, @local_land_id)";
            var battleCommand = new SqlCommand(battleQuery, DbContext.GetSqlConnection());

            battleCommand.Parameters.AddWithValue("@battle_id", battle.BattleId);
            battleCommand.Parameters.AddWithValue("@war_id", battle.WarId);
            battleCommand.Parameters.AddWithValue("@local_land_id", battle.LocalLandId);

            battleCommand.ExecuteNonQuery();

            battleCommand.Dispose();
        }


        public static bool DidTheWarStarted(int index, War war)
        {
            string Query = "SELECT * FROM dbo.BattleData WHERE war_id = @war_id AND local_land_id = @local_land_id";
            var Command = new SqlCommand(Query, DbContext.GetSqlConnection());

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
                reader.Close();
            }

            if (count == 0)
                return false;
            Command.Dispose();
            return true;
        }

        public static List<ArmyInBattle> GetPlayerArmiesInfo(List<ArmyInBattle> armies, War war, Player player)
        {
            string armyQuery = "SELECT * FROM dbo.ArmyDataInBattle WHERE war_id = @war_id AND player_id = @player_id";
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

            var command = new SqlCommand(armyQuery, DbContext.GetSqlConnection());

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
                reader.Close();
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

        public static List<ArmyInBattle> GetAllPlayerArmiesInfo(List<ArmyInBattle> armies, Player player)
        {
            string armyQuery = "SELECT * FROM dbo.ArmyDataInBattle WHERE player_id = @player_id";
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

            var command = new SqlCommand(armyQuery, DbContext.GetSqlConnection());

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
                reader.Close();
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

        public static int CalculateDamageInDistanceBattle(ArmyInBattle _selectedArmy)
        {
            int damage = _selectedArmy.ArmyArchersCount * (int)ForcesEnum.Archers.Damage + _selectedArmy.ArmySiegegunCount * (int)ForcesEnum.Siege.Damage;
            return damage;
        }

        public static DistanceBattle DistanceBattleExist(DistanceBattle _distanceBattle)
        {
            string dbQuery = "SELECT * FROM dbo.DistanceBattleData WHERE local_land_id = @local_land_id AND war_id = @war_id AND side = @side";

            DistanceBattle distanceBattle = new DistanceBattle();
            distanceBattle.BattleId = "";

            var command = new SqlCommand(dbQuery, DbContext.GetSqlConnection());

            command.Parameters.AddWithValue("@local_land_id", _distanceBattle.LocalLandId);
            command.Parameters.AddWithValue("@war_id", _distanceBattle.WarId);
            command.Parameters.AddWithValue("@side", _distanceBattle.Side);

            using (var reader = command.ExecuteReader())
            {
                var battleId = reader.GetOrdinal("battle_id");
                var warId = reader.GetOrdinal("war_id");
                var localLandId = reader.GetOrdinal("local_land_id");
                var damage = reader.GetOrdinal("damage");
                var side = reader.GetOrdinal("side");

                while (reader.Read())
                {
                    distanceBattle.BattleId = reader.GetString(battleId);
                    distanceBattle.WarId = reader.GetString(warId);
                    distanceBattle.LocalLandId = reader.GetInt32(localLandId);
                    distanceBattle.Damage = reader.GetInt32(damage);
                    distanceBattle.Side = reader.GetInt32(side);
                }
                reader.Close();
            }

            command.Dispose();

            return distanceBattle;
        }

        public static void UpdateDistanceBattle(DistanceBattle _distanceBattle)
        {
            string dbQuery = "UPDATE dbo.DistanceBattleData SET damage = @damage WHERE war_id = @war_id AND battle_id = @battle_id";

            var dbCommand = new SqlCommand(dbQuery, DbContext.GetSqlConnection());
            dbCommand.Parameters.AddWithValue("@war_id", _distanceBattle.WarId);
            dbCommand.Parameters.AddWithValue("@battle_id", _distanceBattle.BattleId);
            dbCommand.Parameters.AddWithValue("@damage", _distanceBattle.Damage);

            dbCommand.ExecuteNonQuery();

            dbCommand.Dispose();
        }

        public static void InsertDistanceBattle(DistanceBattle db)
        {
            string dbQuery = "INSERT INTO dbo.DistanceBattleData (battle_id, war_id, local_land_id, damage, side) VALUES (@battle_id, @war_id, @local_land_id, @damage, @side)";
            var command = new SqlCommand(dbQuery, DbContext.GetSqlConnection());

            command.Parameters.AddWithValue("@battle_id", db.BattleId);
            command.Parameters.AddWithValue("@war_id", db.WarId);
            command.Parameters.AddWithValue("@local_land_id", db.LocalLandId);
            command.Parameters.AddWithValue("@damage", db.Damage);
            command.Parameters.AddWithValue("@side", db.Side);

            command.ExecuteNonQuery();

            command.Dispose();
        }

        public static List<Battle> GetInfoAboutBattles(List<Battle> battles, string _warId)
        {
            string dbQuery = "SELECT * FROM dbo.BattleData WHERE war_id = @war_id";
            List<string> battlesBattleId = new List<string>();
            List<string> battlesWarId = new List<string>();
            List<int> battleslocalLandId = new List<int>();

            var command = new SqlCommand(dbQuery, DbContext.GetSqlConnection());
            command.Parameters.AddWithValue("@war_id", _warId);

            using (var reader = command.ExecuteReader())
            {
                var battleId = reader.GetOrdinal("battle_id");
                var warId = reader.GetOrdinal("war_id");
                var localLandId = reader.GetOrdinal("local_land_id");

                while (reader.Read())
                {
                    battles.Add(new Battle());
                    battlesBattleId.Add(reader.GetString(battleId));
                    battlesWarId.Add(reader.GetString(warId));
                    battleslocalLandId.Add(reader.GetInt32(localLandId));
                }
            }

            command.Dispose();

            for (int i = 0; i < battlesBattleId.Count; i++)
            {
                battles[i].BattleId = battlesBattleId[i];
                battles[i].WarId = battlesWarId[i];
                battles[i].LocalLandId = battleslocalLandId[i];
            }

            battlesBattleId = null;
            battlesWarId = null;
            battleslocalLandId = null;

            return battles;
        }
    }
}
