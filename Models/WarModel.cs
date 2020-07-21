using LandConquest.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandConquest.Models
{
    class WarModel
    {
        //public Army UpdateArmy(SqlConnection connection, Army army)
        //{
        //    String storageQuery = "UPDATE dbo.ArmyData SET army_size_current = @army_size_current, army_type  = @army_type, army_archers_count = @army_archers_count, army_infantry_count  = @army_infantry_count, army_horseman_count = @army_horseman_count, army_siegegun_count = @army_siegegun_count, local_land_id = @local_land_id WHERE army_id = @army_id ";

        //    var storageCommand = new SqlCommand(storageQuery, connection);
        //    // int datetimeResult;
        //    storageCommand.Parameters.AddWithValue("@army_size_current", army.ArmySizeCurrent);
        //    storageCommand.Parameters.AddWithValue("@army_type", army.ArmyType);
        //    storageCommand.Parameters.AddWithValue("@army_archers_count", army.ArmyArchersCount);
        //    storageCommand.Parameters.AddWithValue("@army_infantry_count", army.ArmyInfantryCount);
        //    storageCommand.Parameters.AddWithValue("@army_horseman_count", army.ArmyHorsemanCount);
        //    storageCommand.Parameters.AddWithValue("@army_siegegun_count", army.ArmySiegegunCount);
        //    storageCommand.Parameters.AddWithValue("@local_land_id", army.LocalLandId);
        //    storageCommand.Parameters.AddWithValue("@army_id", army.ArmyId);

        //    storageCommand.ExecuteNonQuery();


        //    storageCommand.Dispose();
        //    return army;
        //}
    }
}
