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
            DbContext.GetSqlConnection().Execute("INSERT INTO dbo.LawData (law_id, country_id, operation, player_id, person_id, value1, value2, init_datetime) VALUES (@law_id, @country_id, @operation, @player_id, @person_id, @value1, @value2, @init_datetime)",
                    new
                    {
                        law_id = CommonModel.GenerateId(),
                        country_id = _law.CountryId,
                        operation = _law.Operation,
                        player_id = _law.PlayerId,
                        person_id = _law.PersonId,
                        value1 = _law.Value1,
                        value2 = _law.Value2,
                        init_dateTime = DateTime.UtcNow
                    });
        }

        public static List<LawVote> getLawVotes(string _lawId)
        {
            return DbContext.GetSqlConnection().Query<LawVote>("SELECT * FROM dbo.LawVoteData WHERE law_id = @law_id", new { law_id = _lawId }).ToList();
        }

        public static void insertLawVote(LawVote _lawVote)
        {
            DbContext.GetSqlConnection().Execute("INSERT INTO dbo.LawVoteData (law_id, person_id, player_id, vote_value, vote_datetime) VALUES (@law_id, @person_id, @player_id, @vote_value, @vote_datetime)",
                    new
                    {
                        law_id = _lawVote.LawId,
                        person_id = _lawVote.PersonId,
                        player_id = _lawVote.PlayerId,
                        vote_value = _lawVote.VoteValue,
                        vote_datetime = DateTime.UtcNow
                    });
        }

        public static void deletePersonLawVote(string _lawId, string _personId)
        {
            DbContext.GetSqlConnection().Execute("DELETE FROM dbo.LawVoteData WHERE law_id = @law_id AND person_id = @person_id", new { law_id = _lawId, person_id = _personId});
        }

    }
}
