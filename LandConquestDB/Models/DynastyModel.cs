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
    public class DynastyModel
    {
        public static void CreateDynasty(Dynasty _dynasty)
        {
            string query = "INSERT INTO dbo.DynastyData (dynasty_id, dynasty_name, player_id, prestige, level) " +
                           "VALUES (@dynasty_id, @dynasty_name, @player_id, @prestige, @level)";

            var command = new SqlCommand(query, DbContext.GetSqlConnection());

            command.Parameters.AddWithValue("@dynasty_id", _dynasty.DynastyId);
            command.Parameters.AddWithValue("@dynasty_name", _dynasty.DynastyName);
            command.Parameters.AddWithValue("@player_id", _dynasty.PlayerId);
            command.Parameters.AddWithValue("@prestige", _dynasty.Prestige);
            command.Parameters.AddWithValue("@level", _dynasty.Level);

            command.ExecuteNonQuery();
            command.Dispose();
        }

        public static Dynasty GetDynastyByPlayerId(string _playerId)
        {
            return DbContext.GetSqlConnection().Query<Dynasty>("SELECT * FROM dbo.DynastyData WHERE player_id = @player_id", new { player_id = _playerId }).ToList().FirstOrDefault();
        }
    }
}
