using Dapper;
using LandConquestDB.Entities;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace LandConquestDB.Models
{
    public class TaxesModel
    {
        public static void CreateTaxesData(string userId)
        {
            string taxQuery = "INSERT INTO dbo.TaxesData (player_id, tax_save_datetime) VALUES (@user_id, @tax_save_datetime)";
            var taxCommand = new SqlCommand(taxQuery, DbContext.GetSqlConnection());

            taxCommand.Parameters.AddWithValue("@user_id", userId);
            taxCommand.Parameters.AddWithValue("@tax_save_datetime", DateTime.UtcNow);

            taxCommand.ExecuteNonQuery();
            taxCommand.Dispose();
        }

        public static void SaveTaxes(Taxes taxes)
        {
            string taxesQuery = "UPDATE dbo.TaxesData SET tax_value = @tax_value, tax_money_hour = @tax_money_hour, tax_save_datetime  = @tax_save_datetime WHERE player_id = @player_id ";

            var taxesCommand = new SqlCommand(taxesQuery, DbContext.GetSqlConnection());
            taxesCommand.Parameters.AddWithValue("@tax_value", Convert.ToInt32(taxes.TaxValue));
            taxesCommand.Parameters.AddWithValue("@tax_money_hour", taxes.TaxMoneyHour);
            taxesCommand.Parameters.AddWithValue("@player_id", taxes.PlayerId);
            taxesCommand.Parameters.AddWithValue("@tax_save_datetime", DateTime.UtcNow);

            taxesCommand.ExecuteNonQuery();

            taxesCommand.Dispose();
        }

        public static void SaveTaxes(Taxes taxes, SqlConnection connection)
        {
            string taxesQuery = "UPDATE dbo.TaxesData SET tax_value = @tax_value, tax_money_hour = @tax_money_hour, tax_save_datetime  = @tax_save_datetime WHERE player_id = @player_id ";

            var taxesCommand = new SqlCommand(taxesQuery, connection);
            taxesCommand.Parameters.AddWithValue("@tax_value", Convert.ToInt32(taxes.TaxValue));
            taxesCommand.Parameters.AddWithValue("@tax_money_hour", taxes.TaxMoneyHour);
            taxesCommand.Parameters.AddWithValue("@player_id", taxes.PlayerId);
            taxesCommand.Parameters.AddWithValue("@tax_save_datetime", DateTime.UtcNow);

            taxesCommand.ExecuteNonQuery();

            taxesCommand.Dispose();
        }

        public static Taxes GetTaxesInfo(Taxes tax)
        {
            string query = "SELECT * FROM dbo.TaxesData WHERE player_id = @player_id";

            var taxcommand = new SqlCommand(query, DbContext.GetSqlConnection());
            taxcommand.Parameters.AddWithValue("@player_id", tax.PlayerId);

            using (var reader = taxcommand.ExecuteReader())
            {
                var taxValue = reader.GetOrdinal("tax_value");
                var taxMoneyHour = reader.GetOrdinal("tax_money_hour");
                var taxSaveDateTime = reader.GetOrdinal("tax_save_datetime");


                while (reader.Read())
                {
                    tax.TaxValue = reader.GetInt32(taxValue);
                    tax.TaxMoneyHour = reader.GetInt32(taxMoneyHour);
                    tax.TaxSaveDateTime = reader.GetDateTime(taxSaveDateTime);
                }
                reader.Close();
            }
            taxcommand.Dispose();

            return tax;
        }

        public static Taxes GetTaxesInfo(Taxes tax, SqlConnection connection)
        {
            string query = "SELECT * FROM dbo.TaxesData WHERE player_id = @player_id";

            var taxcommand = new SqlCommand(query, connection);
            taxcommand.Parameters.AddWithValue("@player_id", tax.PlayerId);

            using (var reader = taxcommand.ExecuteReader())
            {
                var taxValue = reader.GetOrdinal("tax_value");
                var taxMoneyHour = reader.GetOrdinal("tax_money_hour");
                var taxSaveDateTime = reader.GetOrdinal("tax_save_datetime");


                while (reader.Read())
                {
                    tax.TaxValue = reader.GetInt32(taxValue);
                    tax.TaxMoneyHour = reader.GetInt32(taxMoneyHour);
                    tax.TaxSaveDateTime = reader.GetDateTime(taxSaveDateTime);
                }
                reader.Close();
            }
            taxcommand.Dispose();

            return tax;
        }

        public static Taxes GetTaxesInfo(string playerId)
        {
            return DbContext.GetSqlConnection().Query<Taxes>("SELECT * FROM dbo.TaxesData WHERE player_id = @player_id", new { player_id = playerId }).ToList().FirstOrDefault();
        }
    }
}
