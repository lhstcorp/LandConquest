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
    public class PersonModel
    {
        public static void CreatePerson(Person _person)
        {
            string query = "INSERT INTO dbo.PersonData (player_id, person_id, name, surname, maleFemale, lvl, exp, power, agility," +
                                                       "health, intellect) " +
                           "VALUES (@player_id, @person_id, @name, @surname, @maleFemale, @lvl, @exp, @power, @agility, @health," +
                                   "@intellect)";
            var command = new SqlCommand(query, DbContext.GetSqlConnection());

            command.Parameters.AddWithValue("@player_id", _person.PlayerId);
            command.Parameters.AddWithValue("@person_id", _person.PersonId);
            command.Parameters.AddWithValue("@name", _person.Name);
            command.Parameters.AddWithValue("@surname", _person.Surname);
            command.Parameters.AddWithValue("@maleFemale", _person.MaleFemale);
            //command.Parameters.AddWithValue("@prestige", _person.Prestige);
            command.Parameters.AddWithValue("@lvl", _person.Lvl);
            command.Parameters.AddWithValue("@exp", _person.Exp);
            command.Parameters.AddWithValue("@power", _person.Power);
            command.Parameters.AddWithValue("@agility", _person.Agility);
            command.Parameters.AddWithValue("@health", _person.Health);
            command.Parameters.AddWithValue("@intellect", _person.Intellect);



            command.ExecuteNonQuery();
            command.Dispose();
        }
        public static Person GetPersonInfo(string _personId)
        {
            return DbContext.GetSqlConnection().Query<Person>("SELECT * FROM dbo.PersonData WHERE person_id = @person_id", new { personId = _personId }).FirstOrDefault();
        }

        public static List<Person> GetPlayerPersons(string _playerId)
        {
            return DbContext.GetSqlConnection().Query<Person>("SELECT * FROM dbo.PersonData WHERE player_id = @player_id ORDER BY lvl DESC", new { player_id = _playerId }).ToList();
        }

        public static void UpdatePersonLvl(Player player, Person person)
        {
            DbContext.GetSqlConnection().Execute("UPDATE dbo.PersonData SET lvl = @lvl WHERE player_id = @player_id", new { lvl = person.Lvl, player_id = player.PlayerId });

        }

        public static void UpdatePersonPower(Player player, Person person)
        {
            DbContext.GetSqlConnection().Execute("UPDATE dbo.PersonData SET power = @power WHERE player_id = @player_id", new { power = person.Power, player_id = player.PlayerId });

        }

        public static void UpdatePersonAgility(Player player, Person person)
        {
            DbContext.GetSqlConnection().Execute("UPDATE dbo.PersonData SET agility = @agility WHERE player_id = @player_id", new { agility = person.Agility, player_id = player.PlayerId });

        }

        public static void UpdatePersonIntellect(Player player, Person person)
        {
            DbContext.GetSqlConnection().Execute("UPDATE dbo.PersonData SET intellect = @intellect WHERE player_id = @player_id", new { intellect = person.Intellect, player_id = player.PlayerId });

        }

        public static void UpdatePersonHealth(Player player, Person person)
        {
            DbContext.GetSqlConnection().Execute("UPDATE dbo.PersonData SET health = @health WHERE player_id = @player_id", new { health = person.Health, player_id = player.PlayerId });

        }
        public static void UpdatePersonExp(Player player, Person person)
        {
            DbContext.GetSqlConnection().Execute("UPDATE dbo.PersonData SET exp = @exp WHERE player_id = @player_id", new { exp = person.Exp, player_id = player.PlayerId });

        }

        public static bool CheckPersonDynastyExistence(string dynasty)
        {
            var player_id = "";
            
            player_id = DbContext.GetSqlConnection().Query<string>("SELECT player_id FROM dbo.PersonData WHERE surname = @surname", new { surname = dynasty}).FirstOrDefault();
            if(String.IsNullOrWhiteSpace(player_id))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static string GetPersonNameSurnameById(string id)
        {
            Person person = DbContext.GetSqlConnection().Query<Person>("SELECT name, surname FROM dbo.PersonData WHERE person_id = @person_id", new { person_id = id }).FirstOrDefault();
            return person.Name + ' ' + person.Surname;
        }
    }
}
