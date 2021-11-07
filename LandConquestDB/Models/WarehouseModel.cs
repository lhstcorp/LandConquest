using LandConquestDB.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandConquestDB.Models
{
    public class WarehouseModel
    {
        public static int? GetWarehouseId(string playerId, int landId)
        {
            int? id = null;
            string query = "SELECT warehouse_id FROM dbo.WarehouseData WHERE player_id = @player_id AND land_id = @land_id";
            var command = new SqlCommand(query, DbContext.GetSqlConnection());

            command.Parameters.AddWithValue("@player_id", playerId);
            command.Parameters.AddWithValue("@land_id", landId);

            using (var reader = command.ExecuteReader())
            {
                var warehouseId = reader.GetOrdinal("warehouse_id");

                while (reader.Read())
                {
                    id = reader.GetInt32(warehouseId);
                }
                reader.Close();
            }

            command.Dispose();

            return id;
        }

        public static void CreateWarehouse(string playerId, int landId)
        {
            string query = "INSERT INTO dbo.WarehouseData (land_id, player_id) VALUES (@land_id, @player_id)";
            var command = new SqlCommand(query, DbContext.GetSqlConnection());

            command.Parameters.AddWithValue("@player_id", playerId);
            command.Parameters.AddWithValue("@land_id", landId);

            command.ExecuteNonQuery();

            command.Dispose();
        }

        public static IEnumerable<StoredItem> GetWarehouseItems(int warehouseId)
        {
            string query =    @"SELECT i.item_id, i.item_name, wi.quantity FROM WarehouseItemData wi
                                JOIN ItemData i
                                  ON wi.item_id = i.item_id
                                WHERE wi.warehouse_id = @warehouse_id";
            var command = new SqlCommand(query, DbContext.GetSqlConnection());

            command.Parameters.AddWithValue("@warehouse_id", warehouseId);

            var list = new List<StoredItem>();

            using (var reader = command.ExecuteReader())
            {
                var itemId = reader.GetOrdinal("item_id");
                var itemName = reader.GetOrdinal("item_name");
                var quantity = reader.GetOrdinal("quantity");

                while (reader.Read())
                {
                    list.Add(new StoredItem
                    {
                        Id = reader.GetInt32(itemId),
                        Name = reader.GetString(itemName).Trim(),
                        Quantity = reader.GetInt32(quantity)
                    });
                }
                reader.Close();
            }

            command.Dispose();

            return list;
        }

        public static void AddItems(int warehouseId, string name, int quantity)
        {
            AddItems(warehouseId, new List<(string name, int quantity)> { (name, quantity) });
        }

        public static void AddItems(int warehouseId, IEnumerable<(string name, int quantity)> items)
        {
            foreach (var item in items)
            {
                string query = @"IF EXISTS (SELECT TOP 1 1 FROM dbo.WarehouseItemData wi 
	                                JOIN dbo.ItemData i ON wi.item_id = i.item_id 
	                                WHERE wi.warehouse_id = @warehouse_id
	                                AND i.item_name = @item_name)
	                                UPDATE wi SET quantity = quantity + @quantity 
	                                FROM dbo.WarehouseItemData wi 
	                                JOIN dbo.ItemData i ON wi.item_id = i.item_id 
	                                WHERE wi.warehouse_id = @warehouse_id
	                                AND i.item_name = @item_name
                                ELSE 
	                                INSERT INTO dbo.WarehouseItemData
		                                (item_id, warehouse_id, quantity)
	                                VALUES
		                                ((SELECT TOP 1 item_id FROM dbo.ItemData WHERE item_name = @item_name), @warehouse_id, @quantity )";
                var command = new SqlCommand(query, DbContext.GetSqlConnection());

                command.Parameters.AddWithValue("@warehouse_id", warehouseId);
                command.Parameters.AddWithValue("@item_name", item.name);
                command.Parameters.AddWithValue("@quantity", item.quantity);

                command.ExecuteNonQuery();

                command.Dispose();
            }
        }

        public static IEnumerable<Warehouse> GetPlayerWarehouses(string playerId)
        {
            string query =    @"SELECT w.warehouse_id, w.land_id, l.land_name FROM dbo.WarehouseData w
                                JOIN dbo.LandData l ON w.land_id = l.land_id
                                WHERE w.player_id = @player_id";
            var command = new SqlCommand(query, DbContext.GetSqlConnection());

            command.Parameters.AddWithValue("@player_id", playerId);

            var list = new List<Warehouse>();

            using (var reader = command.ExecuteReader())
            {
                var warehouseId = reader.GetOrdinal("warehouse_id");
                var landId = reader.GetOrdinal("land_id");
                var landName = reader.GetOrdinal("land_name");

                while (reader.Read())
                {
                    list.Add(new Warehouse
                    {
                        WarehouseId = reader.GetInt32(warehouseId),
                        LandId = reader.GetInt32(landId),
                        PlayerId = playerId,
                        LandName = reader.GetString(landName)
                    });
                }
                reader.Close();
            }

            command.Dispose();

            return list;
        }

        public static IEnumerable<(int id, string name)> GetItems()
        {
            string query = @"SELECT * FROM dbo.ItemData";
            var command = new SqlCommand(query, DbContext.GetSqlConnection());

            var list = new List<(int id, string name)>();

            using (var reader = command.ExecuteReader())
            {
                var itemId = reader.GetOrdinal("item_id");
                var itemName = reader.GetOrdinal("item_name");

                while (reader.Read())
                {
                    list.Add((reader.GetInt32(itemId), reader.GetString(itemName).Trim()));
                }
                reader.Close();
            }

            command.Dispose();

            return list;
        }
    }
}
