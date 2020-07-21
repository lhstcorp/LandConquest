using LandConquest.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandConquest.Models
{
    public class CountryModel
    {
        public Country EstablishaState(SqlConnection connection, Player player, Land land, System.Windows.Media.Color color)
        {
            System.Drawing.Color newColor = System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
            string colorHex = ColorTranslator.ToHtml(newColor);
            String countryQuery = "INSERT INTO dbo.CountryData (country_name,country_ruler,country_color) VALUES (@country_name,@country_ruler,@country_color)";
            var countryCommand = new SqlCommand(countryQuery, connection);

            countryCommand.Parameters.AddWithValue("@country_name", land.LandName + " state");
            countryCommand.Parameters.AddWithValue("@country_ruler", player.PlayerId);
            countryCommand.Parameters.AddWithValue("@country_color", colorHex);

            countryCommand.ExecuteNonQuery();

            Country country = new Country();
            country.CountryName = land.LandName + " state";
            country.CountryRuler = player.PlayerId;
            country.CountryColor = colorHex;

            return country;
        }

        public int SelectLastIdOfStates(SqlConnection connection)
        {
            String stateQuery = "SELECT TOP 1 * FROM dbo.CountryData ORDER BY country_id DESC";
            var stateCommand = new SqlCommand(stateQuery, connection);
            int state_max_id = 1;

            using (var reader = stateCommand.ExecuteReader())
            {
                var stateId = reader.GetOrdinal("country_id");


                while (reader.Read())
                {
                    state_max_id = reader.GetInt32(stateId);
                }
            }

            stateCommand.Dispose();
            return state_max_id;
        }

        public List<Country> GetCountriesInfo(List<Country> countries, SqlConnection connection)
        {
            String query = "SELECT * FROM dbo.CountryData";
            List<Int32> countriesCountryId = new List<Int32>();
            List<string> countriesCountryName = new List<string>();
            List<string> countriesCountryRuler = new List<string>();
            List<string> countriesCountryColor = new List<string>();

            var command = new SqlCommand(query, connection);

            using (var reader = command.ExecuteReader())
            {
                var countryId = reader.GetOrdinal("country_id");
                var countryName = reader.GetOrdinal("country_name");
                var countryRuler = reader.GetOrdinal("country_ruler");
                var countryColor = reader.GetOrdinal("country_color");

                while (reader.Read())
                {
                    countriesCountryId.Add(reader.GetInt32(countryId));
                    countriesCountryName.Add(reader.GetString(countryName));
                    countriesCountryRuler.Add(reader.GetString(countryRuler));
                    countriesCountryColor.Add(reader.GetString(countryColor));
                }
            }

            command.Dispose();

            for (int i = 0; i < countriesCountryId.Count; i++)
            {
                countries[i].CountryId = countriesCountryId[i];
                countries[i].CountryName = countriesCountryName[i];
                countries[i].CountryRuler = countriesCountryRuler[i];
                countries[i].CountryColor = countriesCountryColor[i];
            }

            countriesCountryId = null;
            countriesCountryName = null;
            countriesCountryRuler = null;
            countriesCountryColor = null;

            return countries;
        }

        public int GetCountryId(SqlConnection connection, Player player)
        {
            int id = 0;

            String query = "SELECT country_id FROM dbo.LandData WHERE land_id = @land_id ";

            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@land_id", player.PlayerCurrentRegion);

            using (var reader = command.ExecuteReader())
            {
                var countryId = reader.GetOrdinal("country_id");

                while (reader.Read())
                {
                    id = reader.GetInt32(countryId);
                }
            }

            command.Dispose();

            return id;
        }

        public Country GetCountryById(SqlConnection connection, int id)
        {
            Country country = new Country();

            String query = "SELECT * FROM dbo.CountryData WHERE country_id = @id";

            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);

            using (var reader = command.ExecuteReader())
            {
                var countryId = reader.GetOrdinal("country_id");
                var countryName = reader.GetOrdinal("country_name");
                var countryRuler = reader.GetOrdinal("country_ruler");
                var countryColor = reader.GetOrdinal("country_color");

                while (reader.Read())
                {
                    country.CountryId = reader.GetInt32(countryId);
                    country.CountryName = reader.GetString(countryName);
                    country.CountryRuler = reader.GetString(countryRuler);
                    country.CountryColor = reader.GetString(countryColor);
                }
            }

            command.Dispose();

            return country;
        }

        public void DisbandCountry(SqlConnection connection, Country country)
        {
            String query = "DELETE FROM dbo.CountryData WHERE country_id = @id";

            var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", country.CountryId);

            command.ExecuteNonQuery();

            command.Dispose();
        }
    }
}
