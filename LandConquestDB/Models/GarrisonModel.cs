using LandConquestDB.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace LandConquestDB.Models
{
    public class GarrisonModel
    {
        public static List<Garrison> GetGarrisonInfo(int landId)
        {
            List<Garrison> armies = new List<Garrison>();

            string armyQuery = "SELECT * FROM dbo.GarrisonData WHERE  land_id = @land_id";
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
            List<int> armiesLandId = new List<int>();
            List<int> armiesSlotId = new List<int>();

            var command = new SqlCommand(armyQuery, DbContext.GetSqlConnection());

            command.Parameters.AddWithValue("@land_id", landId);

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
                var armyLandId = reader.GetOrdinal("land_id");
                var armySlotId = reader.GetOrdinal("slot_id");

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
                    armiesLandId.Add(reader.GetInt32(armyLandId));
                    armiesSlotId.Add(reader.GetInt32(armySlotId));
                }
                reader.Close();
            }

            command.Dispose();

            for (int i = 0; i < armiesPlayerId.Count; i++)
            {
                armies.Add(new Garrison());
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
                armies[i].LandId = armiesLandId[i];
                armies[i].SlotId = armiesSlotId[i];
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
            armiesLandId = null;
            armiesSlotId = null;

            return armies;
        }

        public static SolidColorBrush calculateSlotColor(List<Garrison> garrisons, int slotId)
        {
            // написать :)
            return null;
        }
    }
}
