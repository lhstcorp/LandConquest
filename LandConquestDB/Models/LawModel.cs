using Dapper;
using LandConquestDB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandConquestDB.Models
{
    public class LawModel
    {
        public static List<Law> getCountryLaws(int _countryId)
        {
            return DbContext.GetSqlConnection().Query<Law>("SELECT * FROM dbo.LawData WHERE country_id = @country_id", new { country_id = _countryId }).ToList();
        }

        public static void insertLaw(Law _law)
        {
            DbContext.GetSqlConnection().Execute("INSERT INTO dbo.LawData (country_id, operation, player_id, person_id, value1, value2, init_datetime) VALUES (@country_id, @operation, @player_id, @person_id, @value1, @value2, @init_datetime)",
                    new
                    {
                        country_id = _law.CountryId,
                        operation = _law.Operation,
                        player_id = _law.PlayerId,
                        person_id = _law.PersonId,
                        value1 = _law.Value1,
                        value2 = _law.Value2,
                        init_dateTime = DateTime.UtcNow});

        }

    }
}
