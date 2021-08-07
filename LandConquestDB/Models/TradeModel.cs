using LandConquestDB.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandConquestDB.Models
{
    public class TradeModel
    {
        public static void AddTradeOperation(int tradeOperationType, int tradeCenterId, int itemId, int quantity, decimal price, int warehouseId)
        {
            string query = @"INSERT INTO dbo.TradeOperationData (trade_operation_type, trade_center_id, item_id, quantity, price, warehouse_id) 
                            VALUES (@trade_operation_type, @trade_center_id, @item_id, @quantity, @price, @warehouse_id)";
            var command = new SqlCommand(query, DbContext.GetSqlConnection());

            command.Parameters.AddWithValue("@trade_operation_type", tradeOperationType);
            command.Parameters.AddWithValue("@trade_center_id", tradeCenterId);
            command.Parameters.AddWithValue("@item_id", itemId);
            command.Parameters.AddWithValue("@quantity", quantity);
            command.Parameters.AddWithValue("@price", price);
            command.Parameters.AddWithValue("@warehouse_id", warehouseId);

            command.ExecuteNonQuery();

            command.Dispose();
        }

        public static void DeleteTradeOperation(int tradeOperationId)
        {
            string query = "DELETE FROM dbo.TradeOperationData WHERE trade_operation_id = @trade_operation_id";
            var command = new SqlCommand(query, DbContext.GetSqlConnection());

            command.Parameters.AddWithValue("@trade_operation_id", tradeOperationId);

            command.ExecuteNonQuery();

            command.Dispose();
        }

        public static void UpdateTradeOperation(int tradeOperationId, int tradeOperationType, int tradeCenterId, int itemId, int quantity, decimal price, int warehouseId)
        {
            string query = @"UPDATE dbo.TradeOperationData 
                            SET
                                trade_operation_type = @trade_operation_type,
                                trade_center_id = @trade_center_id,
                                item_id = @item_id,
                                quantity = @quantity,
                                price = @price,
                                warehouse_id = @warehouse_id
                            WHERE trade_operation_id = @trade_operation_id";
            var command = new SqlCommand(query, DbContext.GetSqlConnection());

            command.Parameters.AddWithValue("@trade_operation_id", tradeOperationId);
            command.Parameters.AddWithValue("@trade_operation_type", tradeOperationType);
            command.Parameters.AddWithValue("@trade_center_id", tradeCenterId);
            command.Parameters.AddWithValue("@item_id", itemId);
            command.Parameters.AddWithValue("@quantity", quantity);
            command.Parameters.AddWithValue("@price", price);
            command.Parameters.AddWithValue("@warehouse_id", warehouseId);

            command.ExecuteNonQuery();

            command.Dispose();
        }

        public static IEnumerable<TradeOperation> GetWarehouseTradeOperation(int warehouseId)
        {
            string query = @"SELECT * FROM dbo.TradeOperationData WHERE warehouse_id = @warehouse_id";
            var command = new SqlCommand(query, DbContext.GetSqlConnection());

            command.Parameters.AddWithValue("@warehouse_id", warehouseId);

            var list = new List<TradeOperation>();

            using (var reader = command.ExecuteReader())
            {
                var tradeOperationId = reader.GetOrdinal("trade_operation_type");
                var tradeOperationType = reader.GetOrdinal("trade_operation_type");
                var tradeCenterId = reader.GetOrdinal("trade_center_id");
                var itemId = reader.GetOrdinal("item_id");
                var quantity = reader.GetOrdinal("quantity");
                var price = reader.GetOrdinal("price");

                while (reader.Read())
                {
                    list.Add(new TradeOperation
                    {
                        Id = reader.GetInt32(tradeOperationId),
                        TradeOperationType = reader.GetInt32(tradeOperationType),
                        TradeCenterId = reader.GetInt32(tradeCenterId),
                        ItemId = reader.GetInt32(itemId),
                        Quantity = reader.GetInt32(quantity),
                        Price = reader.GetDecimal(price),
                        WarehouseId = warehouseId
                    });
                }
                reader.Close();
            }

            command.Dispose();

            return list;
        }

        public static IEnumerable<(int tradeCenterId, string landName)> GetTradeCenters()
        {
            string query = @"SELECT tc.trade_center_id, l.land_name 
                            FROM dbo.TradeCenterData tc 
                            JOIN dbo.LandData l 
                            ON tc.land_id = l.land_id";
            var command = new SqlCommand(query, DbContext.GetSqlConnection());

            var list = new List<(int tradeCenterId, string landName)>();

            using (var reader = command.ExecuteReader())
            {
                var tradeCenterId = reader.GetOrdinal("trade_center_id");
                var landName = reader.GetOrdinal("land_name");

                while (reader.Read())
                {
                    list.Add((reader.GetInt32(tradeCenterId), reader.GetString(landName).Trim()));
                }
                reader.Close();
            }

            command.Dispose();

            return list;
        }
    }
}
