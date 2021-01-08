using LandConquest.Entities;
using System;
using System.Data.SqlClient;

namespace LandConquest.Models
{
    public class TaxesModel
    {
        public static void CreateTaxesData(String userId)
        {
            String taxQuery = "INSERT INTO dbo.TaxesData (player_id, tax_save_datetime) VALUES (@user_id, @tax_save_datetime)";
            var taxCommand = new SqlCommand(taxQuery, DbContext.GetConnection());

            taxCommand.Parameters.AddWithValue("@user_id", userId);
            taxCommand.Parameters.AddWithValue("@tax_save_datetime", DateTime.UtcNow);

            taxCommand.ExecuteNonQuery();
            taxCommand.Dispose();
        }

        public static void SaveTaxes(Taxes taxes)
        {
            String taxesQuery = "UPDATE dbo.TaxesData SET tax_value = @tax_value, tax_money_hour = @tax_money_hour, tax_save_datetime  = @tax_save_datetime WHERE player_id = @player_id ";

            var taxesCommand = new SqlCommand(taxesQuery, DbContext.GetConnection());
            taxesCommand.Parameters.AddWithValue("@tax_value", Convert.ToInt32(taxes.TaxValue));
            taxesCommand.Parameters.AddWithValue("@tax_money_hour", taxes.TaxMoneyHour);
            taxesCommand.Parameters.AddWithValue("@player_id", taxes.PlayerId);
            taxesCommand.Parameters.AddWithValue("@tax_save_datetime", DateTime.UtcNow);

            taxesCommand.ExecuteNonQuery();

            taxesCommand.Dispose();
        }

        public static Taxes GetTaxesInfo(Taxes tax)
        {
            String query = "SELECT * FROM dbo.TaxesData WHERE player_id = @player_id";

            var taxcommand = new SqlCommand(query, DbContext.GetConnection());
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
            }
            taxcommand.Dispose();

            return tax;
        }
    }
}
