using LandConquest.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandConquest.Models
{
    public class ArmyModel
    {
        public Army EstablishaState(SqlConnection connection, Player player, Army army)
        {
            String armyQuery = "SELECT * FROM dbo.ArmyData (army_id,army_size_current,army_type,army_archers_count,army_infantry_count,army_horseman_count,army_siegegun_count,local_land_id) VALUES (@army_id,@army_size_current,@army_type,@army_archers_count,@army_infantry_count,@army_horseman_count,@army_siegegun_count,@local_land_id) WHERE player_id = @player_id";
            var armyCommand = new SqlCommand(armyQuery, connection);

            armyCommand.Parameters.AddWithValue("@player_id", player.PlayerId);

            using (var reader = armyCommand.ExecuteReader())
            {
                var armyId = reader.GetOrdinal("army_id");
                var armySizeCurrent = reader.GetOrdinal("army_size_current");
                var armyType = reader.GetOrdinal("army_type");
                var armyArchersCount = reader.GetOrdinal("army_archers_count");
                var armyInfantryCount = reader.GetOrdinal("army_infantry_count");
                var armyHorsemanCount = reader.GetOrdinal("army_horseman_count");
                var armySiegegunCount = reader.GetOrdinal("army_siegegun_count");
                var localLandId = reader.GetOrdinal("local_land_id");

                while (reader.Read())
                {
                    army.ArmyId = reader.GetString(armyId);
                    army.ArmySizeCurrent = reader.GetInt32(armySizeCurrent);
                    army.ArmyType = reader.GetInt32(armyType);
                    army.ArmyArchersCount = reader.GetInt32(armyArchersCount);
                    army.ArmyInfantryCount = reader.GetInt32(armyInfantryCount);
                    army.ArmyHorsemanCount = reader.GetInt32(armyHorsemanCount);
                    army.ArmySiegegunCount = reader.GetInt32(armySiegegunCount);
                    army.LocalLandId = reader.GetInt32(localLandId);
                }
            }
            army.PlayerId = player.PlayerId;
            armyCommand.Dispose();

            return army;
        }
    }
}
