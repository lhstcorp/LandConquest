using LandConquestServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace LandConquestDB.Models
{
    public class WarScoreModel
    {
        public static WarScore getPlayerWarScoreInWar(string _playerId, string _warId)
        {
            return DbContext.GetSqlConnection().Query<WarScore>("SELECT * FROM dbo.WarScoreData WHERE player_id = @player_id AND war_id = @war_id", new { player_id = _playerId, war_id = _warId }).ToList().FirstOrDefault();
        }

    }
}
