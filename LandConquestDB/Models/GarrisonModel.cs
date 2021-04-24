using LandConquestDB.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
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

            string armyQuery = "SELECT * FROM dbo.GarrisonData WHERE land_id = @land_id";
            List<string> armiesPlayerId = new List<string>();
            List<string> armiesArmyId = new List<string>();
            List<int> armiesSizeCurrent = new List<int>();
            List<int> armiesArmyArchersCount = new List<int>();
            List<int> armiesArmyInfantryCount = new List<int>();
            List<int> armiesArmyHorsemanCount = new List<int>();
            List<int> armiesArmySiegegunCount = new List<int>();
            List<int> armiesLandId = new List<int>();
            List<int> armiesSlotId = new List<int>();

            var command = new SqlCommand(armyQuery, DbContext.GetSqlConnection());

            command.Parameters.AddWithValue("@land_id", landId);

            using (var reader = command.ExecuteReader())
            {
                var playerId = reader.GetOrdinal("player_id");
                var armyId = reader.GetOrdinal("army_id");
                var armySizeCurrent = reader.GetOrdinal("army_size_current");
                var armyArchersCount = reader.GetOrdinal("army_archers_count");
                var armyInfantryCount = reader.GetOrdinal("army_infantry_count");
                var armyHorsemanCount = reader.GetOrdinal("army_horseman_count");
                var armySiegegunCount = reader.GetOrdinal("army_siegegun_count");
                var armyLandId = reader.GetOrdinal("land_id");
                var armySlotId = reader.GetOrdinal("slot_id");

                while (reader.Read())
                {
                    armiesPlayerId.Add(reader.GetString(playerId));
                    armiesArmyId.Add(reader.GetString(armyId));
                    armiesSizeCurrent.Add(reader.GetInt32(armySizeCurrent));
                    armiesArmyArchersCount.Add(reader.GetInt32(armyArchersCount));
                    armiesArmyInfantryCount.Add(reader.GetInt32(armyInfantryCount));
                    armiesArmyHorsemanCount.Add(reader.GetInt32(armyHorsemanCount));
                    armiesArmySiegegunCount.Add(reader.GetInt32(armySiegegunCount));
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
                armies[i].ArmyArchersCount = armiesArmyArchersCount[i];
                armies[i].ArmyInfantryCount = armiesArmyInfantryCount[i];
                armies[i].ArmyHorsemanCount = armiesArmyHorsemanCount[i];
                armies[i].ArmySiegegunCount = armiesArmySiegegunCount[i];
                armies[i].LandId = armiesLandId[i];
                armies[i].SlotId = armiesSlotId[i];
            }

            armiesPlayerId = null;
            armiesArmyId = null;
            armiesSizeCurrent = null;
            armiesArmyArchersCount = null;
            armiesArmyInfantryCount = null;
            armiesArmyHorsemanCount = null;
            armiesArmySiegegunCount = null;
            armiesSlotId = null;
            armiesLandId = null;

            return armies;
        }

        public static List<Garrison> GetPlayerGarrisonInfo(string _playerId)
        {
            List<Garrison> armies = new List<Garrison>();

            string armyQuery = "SELECT * FROM dbo.GarrisonData WHERE player_id = @player_id";
            List<string> armiesPlayerId = new List<string>();
            List<string> armiesArmyId = new List<string>();
            List<int> armiesSizeCurrent = new List<int>();
            List<int> armiesArmyArchersCount = new List<int>();
            List<int> armiesArmyInfantryCount = new List<int>();
            List<int> armiesArmyHorsemanCount = new List<int>();
            List<int> armiesArmySiegegunCount = new List<int>();
            List<int> armiesLandId = new List<int>();
            List<int> armiesSlotId = new List<int>();

            var command = new SqlCommand(armyQuery, DbContext.GetSqlConnection());

            command.Parameters.AddWithValue("@player_id", _playerId);

            using (var reader = command.ExecuteReader())
            {
                var playerId = reader.GetOrdinal("player_id");
                var armyId = reader.GetOrdinal("army_id");
                var armySizeCurrent = reader.GetOrdinal("army_size_current");
                var armyArchersCount = reader.GetOrdinal("army_archers_count");
                var armyInfantryCount = reader.GetOrdinal("army_infantry_count");
                var armyHorsemanCount = reader.GetOrdinal("army_horseman_count");
                var armySiegegunCount = reader.GetOrdinal("army_siegegun_count");
                var armyLandId = reader.GetOrdinal("land_id");
                var armySlotId = reader.GetOrdinal("slot_id");

                while (reader.Read())
                {
                    armiesPlayerId.Add(reader.GetString(playerId));
                    armiesArmyId.Add(reader.GetString(armyId));
                    armiesSizeCurrent.Add(reader.GetInt32(armySizeCurrent));
                    armiesArmyArchersCount.Add(reader.GetInt32(armyArchersCount));
                    armiesArmyInfantryCount.Add(reader.GetInt32(armyInfantryCount));
                    armiesArmyHorsemanCount.Add(reader.GetInt32(armyHorsemanCount));
                    armiesArmySiegegunCount.Add(reader.GetInt32(armySiegegunCount));
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
                armies[i].ArmyArchersCount = armiesArmyArchersCount[i];
                armies[i].ArmyInfantryCount = armiesArmyInfantryCount[i];
                armies[i].ArmyHorsemanCount = armiesArmyHorsemanCount[i];
                armies[i].ArmySiegegunCount = armiesArmySiegegunCount[i];
                armies[i].LandId = armiesLandId[i];
                armies[i].SlotId = armiesSlotId[i];
            }

            armiesPlayerId = null;
            armiesArmyId = null;
            armiesSizeCurrent = null;
            armiesArmyArchersCount = null;
            armiesArmyInfantryCount = null;
            armiesArmyHorsemanCount = null;
            armiesArmySiegegunCount = null;
            armiesSlotId = null;
            armiesLandId = null;

            return armies;
        }

        public static int calculateSlotColor(List<Garrison> garrisons, int slotId, Castle castle)
        {
            int colorIndex = 0;
            Garrison garrison = new Garrison();

            for (int i = 0; i < garrisons.Count; i++)
            {
                if (garrisons[i].SlotId == slotId)
                {
                    garrison.ArmySizeCurrent += garrisons[i].ArmySizeCurrent;
                }
            }

            int maxTroops = castle.returnMaxTroopsInSlot();

            float share = ((float)garrison.ArmySizeCurrent / (float)maxTroops);

            if (share <= 0.1)
            {
                colorIndex = 0;
            } 
            else if (share <= 0.2)
            {
                colorIndex = 1;
            }
            else if (share <= 0.3)
            {
                colorIndex = 2;
            }
            else if (share <= 0.4)
            {
                colorIndex = 3;
            }
            else if (share <= 0.5)
            {
                colorIndex = 4;
            }
            else if (share <= 0.6)
            {
                colorIndex = 5;
            }
            else if (share <= 0.7)
            {
                colorIndex = 6;
            }
            else if (share <= 0.8)
            {
                colorIndex = 7;
            }
            else if (share <= 0.9)
            {
                colorIndex = 8;
            }
            else if (share <= 1.0)
            {
                colorIndex = 9;
            }

            // написать :)
            return colorIndex;
        }

        public static void InsertGarrison(Garrison army)
        {
            string armyQuery = "INSERT INTO dbo.GarrisonData (land_id, player_id, army_id, army_size_current, army_type, army_archers_count, army_infantry_count, army_horseman_count, army_siegegun_count, slot_id) VALUES (@land_id, @player_id, @army_id, @army_size_current, @army_type, @army_archers_count, @army_infantry_count, @army_horseman_count, @army_siegegun_count, @slot_id)";
            var armyCommand = new SqlCommand(armyQuery, DbContext.GetSqlConnection());

            armyCommand.Parameters.AddWithValue("@land_id", army.LandId);
            armyCommand.Parameters.AddWithValue("@player_id", army.PlayerId);
            armyCommand.Parameters.AddWithValue("@army_id", army.ArmyId);
            armyCommand.Parameters.AddWithValue("@army_size_current", army.ArmySizeCurrent);
            armyCommand.Parameters.AddWithValue("@army_type", army.ArmyType);
            armyCommand.Parameters.AddWithValue("@army_archers_count", army.ArmyArchersCount);
            armyCommand.Parameters.AddWithValue("@army_infantry_count", army.ArmyInfantryCount);
            armyCommand.Parameters.AddWithValue("@army_horseman_count", army.ArmyHorsemanCount);
            armyCommand.Parameters.AddWithValue("@army_siegegun_count", army.ArmySiegegunCount);
            armyCommand.Parameters.AddWithValue("@slot_id", army.SlotId);

            armyCommand.ExecuteNonQuery();

            armyCommand.Dispose();
        }

        public static Uri returnTypeImg(int inf, int ar, int kn, int sie)
        {
            int total = inf + ar + kn + sie;
            Uri uri;

            if (inf == total)
            {
                uri = new Uri("/Pictures/Armies/INF-0.png", UriKind.Relative);
            }
            else if (ar == total)
            {
                uri = new Uri("/Pictures/Armies/AR-0.png", UriKind.Relative);
            }
            else if (kn == total)
            {
                uri = new Uri("/Pictures/Armies/KNT-0.png", UriKind.Relative);
            }
            else if (sie == total)
            {
                uri = new Uri("/Pictures/Armies/SIE-0.png", UriKind.Relative);
            }
            else
            {
                uri = new Uri("/Pictures/Armies/TRO-0.png", UriKind.Relative);
            }

            return uri;
        }

        public static void deleteGarrisonById(string armyId)
        {
            string query = "DELETE FROM dbo.GarrisonData WHERE army_id = @army_id";

            var command = new SqlCommand(query, DbContext.GetSqlConnection());
            command.Parameters.AddWithValue("@army_id", armyId);

            command.ExecuteNonQuery();

            command.Dispose();
        }
    }
}
