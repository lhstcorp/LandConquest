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
            string query = "INSERT INTO dbo.PersonData (player_id, person_id, name, surname, maleFemale, power, agility," +
                                                       "health, intellect) " +
                           "VALUES (@player_id, @person_id, @name, @surname, @maleFemale, @power, @agility, @health," +
                                   "@intellect)";
            var command = new SqlCommand(query, DbContext.GetSqlConnection());

            command.Parameters.AddWithValue("@player_id", _person.PlayerId);
            command.Parameters.AddWithValue("@person_id", _person.PersonId);
            command.Parameters.AddWithValue("@name", _person.Name);
            command.Parameters.AddWithValue("@surname", _person.Surname);
            command.Parameters.AddWithValue("@maleFemale", _person.MaleFemale);
            //command.Parameters.AddWithValue("@prestige", _person.Prestige);
            //command.Parameters.AddWithValue("@lvl", _person.Level);
            //command.Parameters.AddWithValue("@exp", _person.Exp);
            command.Parameters.AddWithValue("@power", _person.Power);
            command.Parameters.AddWithValue("@agility", _person.Agility);
            command.Parameters.AddWithValue("@health", _person.Health);
            command.Parameters.AddWithValue("@intellect", _person.Intellect);



            command.ExecuteNonQuery();
            command.Dispose();
        }
        public static Person GetPersonInfo(Player player, Person person)
        {
            string query = "SELECT * FROM dbo.PersonData WHERE person_id = @person_id";

            var command = new SqlCommand(query, DbContext.GetSqlConnection());
            command.Parameters.AddWithValue("@person_id", player.PlayerId);

            using (var reader = command.ExecuteReader())
            {
                var playerId = reader.GetOrdinal("player_id");
                var personId = reader.GetOrdinal("person_id");
                var name = reader.GetOrdinal("name");
                var surname = reader.GetOrdinal("surname");
                var maleFemale = reader.GetOrdinal("maleFemale");
                var power = reader.GetOrdinal("power");
                var agility = reader.GetOrdinal("agility");
                var health = reader.GetOrdinal("health");
                var intellect = reader.GetOrdinal("intellect");

                while (reader.Read())
                {
                    player.PlayerId = reader.GetString(playerId);
                    person.PersonId = reader.GetString(personId);
                    person.Name = reader.GetString(name);
                    //person.Surname = reader.GetString(surname);
                    person.MaleFemale = reader.GetInt32(maleFemale)>0;
                    person.Power = reader.GetInt32(power);
                    person.Agility = reader.GetInt32(agility);
                    person.Health = reader.GetInt32(health);
                    person.Intellect = reader.GetInt32(intellect);
                }
                reader.Close();
            }
            command.Dispose();

            return person;
        }
    }
}
