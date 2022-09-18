using System;
using System.Collections.Generic;
using System.Linq;
using LandConquestDB.Entities;
using Dapper;

namespace LandConquestDB.Models
{
    public class ItemModel
    {
        public static List<Item> getItems()
        {
            return DbContext.GetSqlConnection().Query<Item>("SELECT * FROM dbo.ItemData", new { }).ToList();
        }

        public static List<Item> getItemsByCategory(int _category)
        {
            return DbContext.GetSqlConnection().Query<Item>("SELECT * FROM dbo.ItemData WHERE item_type = @category", new { category = _category }).ToList();
        }

        public static List<ItemReceipt> getItemReceiptsByProducedItemId(int _producedItemId)
        {
            return DbContext.GetSqlConnection().Query<ItemReceipt>("SELECT * FROM dbo.ItemReceiptData WHERE produced_item_id = @produced_item_id", new { produced_item_id = _producedItemId }).ToList();
        }

        
    }
}
