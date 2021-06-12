using LandConquestDB.Entities;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;

namespace LandConquestDB.Models
{
    public class CountryModel
    {
        public static Country EstablishaState(Player player, Land land, System.Windows.Media.Color color)
        {
            Color newColor = Color.FromArgb(color.A, color.R, color.G, color.B);
            string colorHex = ColorTranslator.ToHtml(newColor);
            string coffers = "10000";
            string capitalId = land.LandId.ToString();
            string countryQuery = "INSERT INTO dbo.CountryData (country_name,country_ruler,country_color,country_coffers, capital_id) VALUES (@country_name,@country_ruler,@country_color,@country_coffers, @capital_id)";
            var countryCommand = new SqlCommand(countryQuery, DbContext.GetSqlConnection());

            countryCommand.Parameters.AddWithValue("@country_name", land.LandName + " state");
            countryCommand.Parameters.AddWithValue("@country_ruler", player.PlayerId);
            countryCommand.Parameters.AddWithValue("@country_color", colorHex);
            countryCommand.Parameters.AddWithValue("@country_coffers", coffers);
            countryCommand.Parameters.AddWithValue("@capital_id", capitalId);

            countryCommand.ExecuteNonQuery();
            countryCommand.Dispose();

            Country country = new Country();
            country.CountryName = land.LandName + " state";
            country.CountryRuler = player.PlayerId;
            country.CountryColor = colorHex;
            country.CountryCoffers = coffers;
            country.CapitalId = capitalId;

            return country;
        }

        public static int SelectLastIdOfStates()
        {
            string stateQuery = "SELECT TOP 1 * FROM dbo.CountryData ORDER BY country_id DESC";
            var stateCommand = new SqlCommand(stateQuery, DbContext.GetSqlConnection());
            int state_max_id = 1;

            using (var reader = stateCommand.ExecuteReader())
            {
                var stateId = reader.GetOrdinal("country_id");


                while (reader.Read())
                {
                    state_max_id = reader.GetInt32(stateId);
                }
                reader.Close();
            }
            stateCommand.Dispose();
            return state_max_id;
        }

        public static List<Country> GetCountriesInfo(List<Country> countries)
        {
            string query = "SELECT * FROM dbo.CountryData";
            List<int> countriesCountryId = new List<int>();
            List<string> countriesCountryName = new List<string>();
            List<string> countriesCountryRuler = new List<string>();
            List<string> countriesCountryColor = new List<string>();
            List<string> countriesCapitalId = new List<string>();

            var command = new SqlCommand(query, DbContext.GetSqlConnection());

            using (var reader = command.ExecuteReader())
            {
                var countryId = reader.GetOrdinal("country_id");
                var countryName = reader.GetOrdinal("country_name");
                var countryRuler = reader.GetOrdinal("country_ruler");
                var countryColor = reader.GetOrdinal("country_color");
                var capitalId = reader.GetOrdinal("capital_id");

                while (reader.Read())
                {
                    countriesCountryId.Add(reader.GetInt32(countryId));
                    countriesCountryName.Add(reader.GetString(countryName));
                    countriesCountryRuler.Add(reader.GetString(countryRuler));
                    countriesCountryColor.Add(reader.GetString(countryColor));
                    countriesCapitalId.Add(reader.GetString(capitalId));
                }
                reader.Close();
            }

            command.Dispose();

            for (int i = 0; i < countriesCountryId.Count; i++)
            {
                countries[i].CountryId = countriesCountryId[i];
                countries[i].CountryName = countriesCountryName[i];
                countries[i].CountryRuler = countriesCountryRuler[i];
                countries[i].CountryColor = countriesCountryColor[i];
                countries[i].CapitalId = countriesCapitalId[i];
            }

            countriesCountryId = null;
            countriesCountryName = null;
            countriesCountryRuler = null;
            countriesCountryColor = null;
            countriesCapitalId = null;

            return countries;
        }

        public static int GetCountryIdByLandId(int _landId)
        {
            int id = 0;

            string query = "SELECT country_id FROM dbo.LandData WHERE land_id = @land_id ";

            var command = new SqlCommand(query, DbContext.GetSqlConnection());
            command.Parameters.AddWithValue("@land_id", _landId);

            using (var reader = command.ExecuteReader())
            {
                var countryId = reader.GetOrdinal("country_id");

                while (reader.Read())
                {
                    id = reader.GetInt32(countryId);
                }
                reader.Close();
            }

            command.Dispose();

            return id;
        }

        public static string GetCountryRuler(int id)
        {
            string rulerId = "";
            string query = "SELECT country_ruler FROM dbo.CountryData WHERE country_id = @country_id ";

            var command = new SqlCommand(query, DbContext.GetSqlConnection());
            command.Parameters.AddWithValue("@country_id", id);

            using (var reader = command.ExecuteReader())
            {
                var countryRuler = reader.GetOrdinal("country_ruler");

                while (reader.Read())
                {
                    rulerId = reader.GetString(countryRuler);
                }
                reader.Close();
            }
            command.Dispose();
            return rulerId;
        }

        public static Country GetCountryById(int id)
        {
            Country country = new Country();

            string query = "SELECT * FROM dbo.CountryData WHERE country_id = @id";

            var command = new SqlCommand(query, DbContext.GetSqlConnection());
            command.Parameters.AddWithValue("@id", id);

            using (var reader = command.ExecuteReader())
            {
                var countryId = reader.GetOrdinal("country_id");
                var countryName = reader.GetOrdinal("country_name");
                var countryRuler = reader.GetOrdinal("country_ruler");
                var countryColor = reader.GetOrdinal("country_color");
                var countryCoffers = reader.GetOrdinal("country_coffers");
                var capitalId = reader.GetOrdinal("capital_id"); 

                while (reader.Read())
                {
                    country.CountryId = reader.GetInt32(countryId);
                    country.CountryName = reader.GetString(countryName);
                    country.CountryRuler = reader.GetString(countryRuler);
                    country.CountryColor = reader.GetString(countryColor);
                    country.CountryCoffers = reader.GetString(countryCoffers);
                    country.CapitalId = reader.GetString(capitalId);
                }
                reader.Close();
            }

            command.Dispose();

            return country;
        }


        public static void DisbandCountry(Country country)
        {
            string query = "DELETE FROM dbo.CountryData WHERE country_id = @id";

            var command = new SqlCommand(query, DbContext.GetSqlConnection());
            command.Parameters.AddWithValue("@id", country.CountryId);

            command.ExecuteNonQuery();

            command.Dispose();
        }
    }
}
