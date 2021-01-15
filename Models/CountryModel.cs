using LandConquest.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;

namespace LandConquest.Models
{
    public class CountryModel
    {
        public static Country EstablishaState(Player player, Land land, System.Windows.Media.Color color)
        {
            Color newColor = Color.FromArgb(color.A, color.R, color.G, color.B);
            string colorHex = ColorTranslator.ToHtml(newColor);
            string coffers = "10000";
            String countryQuery = "INSERT INTO dbo.CountryData (country_name,country_ruler,country_color,country_coffers) VALUES (@country_name,@country_ruler,@country_color,@country_coffers)";
            var countryCommand = new SqlCommand(countryQuery, DbContext.GetConnection());

            countryCommand.Parameters.AddWithValue("@country_name", land.LandName + " state");
            countryCommand.Parameters.AddWithValue("@country_ruler", player.PlayerId);
            countryCommand.Parameters.AddWithValue("@country_color", colorHex);
            countryCommand.Parameters.AddWithValue("@country_coffers", coffers);

            countryCommand.ExecuteNonQuery();

            Country country = new Country();
            country.CountryName = land.LandName + " state";
            country.CountryRuler = player.PlayerId;
            country.CountryColor = colorHex;
            country.CountryCoffers = coffers;

            return country;
        }

        public static int SelectLastIdOfStates()
        {
            String stateQuery = "SELECT TOP 1 * FROM dbo.CountryData ORDER BY country_id DESC";
            var stateCommand = new SqlCommand(stateQuery, DbContext.GetConnection());
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

        public static List<Country> GetCountriesInfo(List<Country> countries)
        {
            String query = "SELECT * FROM dbo.CountryData";
            List<Int32> countriesCountryId = new List<Int32>();
            List<string> countriesCountryName = new List<string>();
            List<string> countriesCountryRuler = new List<string>();
            List<string> countriesCountryColor = new List<string>();

            var command = new SqlCommand(query, DbContext.GetConnection());

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

        public static int GetCountryId(Player player)
        {
            int id = 0;

            String query = "SELECT country_id FROM dbo.LandData WHERE land_id = @land_id ";

            var command = new SqlCommand(query, DbContext.GetConnection());
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

        public static Country GetCountryById(int id)
        {
            Country country = new Country();

            String query = "SELECT * FROM dbo.CountryData WHERE country_id = @id";

            var command = new SqlCommand(query, DbContext.GetConnection());
            command.Parameters.AddWithValue("@id", id);

            using (var reader = command.ExecuteReader())
            {
                var countryId = reader.GetOrdinal("country_id");
                var countryName = reader.GetOrdinal("country_name");
                var countryRuler = reader.GetOrdinal("country_ruler");
                var countryColor = reader.GetOrdinal("country_color");
                var countryCoffers = reader.GetOrdinal("country_coffers");

                while (reader.Read())
                {
                    country.CountryId = reader.GetInt32(countryId);
                    country.CountryName = reader.GetString(countryName);
                    country.CountryRuler = reader.GetString(countryRuler);
                    country.CountryColor = reader.GetString(countryColor);
                    country.CountryCoffers = reader.GetString(countryCoffers);
                }
            }

            command.Dispose();

            return country;
        }

        
        public static void DisbandCountry(Country country)
        {
            String query = "DELETE FROM dbo.CountryData WHERE country_id = @id";

            var command = new SqlCommand(query, DbContext.GetConnection());
            command.Parameters.AddWithValue("@id", country.CountryId);

            command.ExecuteNonQuery();

            command.Dispose();
        }
    }
}
