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
        public List<ArmyInBattle> GetArmiesInfo(SqlConnection connection, List<ArmyInBattle> armies)
        {
            String armyQuery = "SELECT * FROM dbo.ArmyDataInBattle";
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

        public void InsertArmyIntoBattleTable(SqlConnection connection, ArmyInBattle army)
        {
            String armyQuery = "INSERT INTO dbo.ArmyDataInBattle (player_id, army_id, army_size_current, army_type, army_archers_count, army_infantry_count, army_horseman_count, army_siegegun_count, local_land_id, army_side) VALUES (@player_id, @army_id, @army_size_current, @army_type, @army_archers_count, @army_infantry_count, @army_horseman_count, @army_siegegun_count, @local_land_id, @army_side)";
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

            armyCommand.ExecuteNonQuery();

            armyCommand.Dispose();
        }

        public int SelectLastIdOfArmies(SqlConnection connection)
        {
            String Query = "SELECT * FROM dbo.ArmyDataInBattle";
            var Command = new SqlCommand(Query, connection);
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
    }
}
