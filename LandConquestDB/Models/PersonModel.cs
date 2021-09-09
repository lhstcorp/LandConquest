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
            string query = "INSERT INTO dbo.PersonData (player_id, person_id, name, surname, maleFemale) " +
                           "VALUES (@player_id, @person_id, @name, @surname, @maleFemale)";
            var command = new SqlCommand(query, DbContext.GetSqlConnection());

            command.Parameters.AddWithValue("@player_id", _person.PlayerId);
            command.Parameters.AddWithValue("@person_id", _person.PersonId);
            command.Parameters.AddWithValue("@name", _person.Name);
            command.Parameters.AddWithValue("@surname", _person.Surname);
            command.Parameters.AddWithValue("@maleFemale", _person.MaleFemale);
            command.Parameters.AddWithValue("@prestige", _person.Prestige);
            command.Parameters.AddWithValue("@level", _person.Level);
            command.Parameters.AddWithValue("@exp", _person.Exp);
            command.Parameters.AddWithValue("@exp", _person.Power);
            command.Parameters.AddWithValue("@exp", _person.Agility);
            command.Parameters.AddWithValue("@exp", _person.Health);
            command.Parameters.AddWithValue("@exp", _person.Intellect);

            command.ExecuteNonQuery();
            command.Dispose();
        }
    }
}
