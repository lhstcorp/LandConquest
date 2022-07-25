using Dapper;
using LandConquestDB.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandConquestDB.Models
{
    public class BuildingsModel
    {
        public static Buildings GetPlayerBuildings(string _playerId, int _landId)
        {
            return DbContext.GetSqlConnection().Query<Buildings>("SELECT * FROM dbo.BuildingsData WHERE player_id = @player_id AND land_id = @land_id", new { player_id = _playerId, land_id = _landId }).ToList().FirstOrDefault();
        }

        public static int GetSumLandPopulation(int _landId)
        {
            return DbContext.GetSqlConnection().Query<int>("SELECT SUM(houses) FROM dbo.BuildingsData WHERE land_id = @land_id", new { land_Id = _landId }).Sum();

        }

        public static void CreateBuildings(string _playerId, int _landId)
        {
            string query = "INSERT INTO dbo.BuildingsData (player_id, land_id) " +
                           "VALUES (@player_id, @land_id)";
            var command = new SqlCommand(query, DbContext.GetSqlConnection());

            command.Parameters.AddWithValue("@player_id", _playerId);
            command.Parameters.AddWithValue("@land_id", _landId);

            command.ExecuteNonQuery();
            command.Dispose();
        }

        public static void UpdateBuildings(Buildings _buildings)
        {
            string Query = "UPDATE dbo.BuildingsData SET land_id  = @land_id, houses = @houses WHERE player_id = @player_id ";

            var Command = new SqlCommand(Query, DbContext.GetSqlConnection());

            Command.Parameters.AddWithValue("@player_id", _buildings.PlayerId);
            Command.Parameters.AddWithValue("@land_id", _buildings.LandId);
            Command.Parameters.AddWithValue("@houses", _buildings.Houses);

            Command.ExecuteNonQuery();

            Command.Dispose();
        }
    }
}
