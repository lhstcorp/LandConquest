﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using LandConquestDB.Entities;

namespace LandConquestDB.Models
{
    public class BuildingsModel
    {
        public static Buildings GetBuildingsInfo(string playerId)
        {
            Buildings buildings = new Buildings();
            string query = "SELECT * FROM dbo.BuildingsData where player_id = @player_id";

            var command = new SqlCommand(query, DbContext.GetSqlConnection());
            command.Parameters.AddWithValue("@playerId", playerId);

            using (var reader = command.ExecuteReader())
            {
                var buildingsPlayerId = reader.GetOrdinal("player_id");
                var buildingsLandId = reader.GetOrdinal("land_id");
                var housesCount = reader.GetOrdinal("houses");

                while (reader.Read())
                {
                    buildings.LandId = reader.GetInt32(buildingsLandId);
                    buildings.PlayerId = reader.GetInt32(buildingsPlayerId);
                    buildings.Houses = reader.GetInt32(housesCount);
                }
                reader.Close();
            }

            command.Dispose();

            return buildings;
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