using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandConquestDB.Entities
{
    public class ItemReceipt
    {
        public int ProducedItemId { get; set; }
        public float ProducedItemQty { get; set; }
        public int ReceiptItemId { get; set; }
        public float ReceiptItemQty { get; set; }
    }
}
