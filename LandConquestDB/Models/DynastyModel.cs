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
        public static void CreateDynasty(User _user)
        {
            //DbContext.GetSqlConnection().Execute("INSERT INTO dbo.DynastyData (dynasty_id, dynasty_name, player_id, prestige, lvl) " +
            //               "VALUES (@dynasty_id, @dynasty_name, @player_id, @prestige, @lvl)", new { dynasty_id = CommonModel.GenerateId(), dynasty_name = _user.UserLogin, player_id = _user.UserId, prestige = 0, lvl = 1 });

            string query = "INSERT INTO dbo.DynastyData (dynasty_id, dynasty_name, player_id, prestige, lvl) " +
                           "VALUES (@dynasty_id, @dynasty_name, @player_id, @prestige, @lvl)";

            var command = new SqlCommand(query, DbContext.GetSqlConnection());

            command.Parameters.AddWithValue("@dynasty_id", CommonModel.GenerateId());
            command.Parameters.AddWithValue("@dynasty_name", _user.UserLogin);
            command.Parameters.AddWithValue("@player_id", _user.UserId);
            command.Parameters.AddWithValue("@prestige", 0);
            command.Parameters.AddWithValue("@lvl", 1);

            command.ExecuteNonQuery();
            command.Dispose();
        }

        public static Dynasty GetDynastyByPlayerId(string _playerId)
        {
            return DbContext.GetSqlConnection().Query<Dynasty>("SELECT * FROM dbo.DynastyData WHERE player_id = @player_id", new { player_id = _playerId }).ToList().FirstOrDefault();
        }
    }
}
