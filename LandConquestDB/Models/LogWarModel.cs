using Dapper;
using LandConquestDB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandConquestDB.Models
{
    public class LogWarModel
    {
        public static List<LogWar> GetLogWarList(string _warId)
        {
            return DbContext.GetSqlConnection().Query<LogWar>("SELECT * FROM dbo.LogWarData WHERE war_id = @war_id ORDER BY creation_time DESC", new { war_id = _warId }).ToList();
        }
    }
}
