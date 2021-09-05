using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandConquestDB.Entities
{
    public class TradeOperation
    {
        public int Id { get; set; }
        //tradeOperationType:
        //1: покупать [quantity] каждую сессию
        //2: продавать [quantity] каждую сессию
        //3: покупать до [quantity] каждую сессию
        //4: продавать всё свыше [quantity] каждую сессию
        public int TradeOperationType { get; set; }
        public int TradeCenterId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int WarehouseId { get; set; }
    }
}
