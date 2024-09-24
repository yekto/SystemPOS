using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemPOS.Shared.POS
{
    public class POSmodel
    {
        public string itemId { get; set; }
        public string itemName { get; set; }
        public decimal price { get; set; }
        public string category {  get; set; }
        public decimal qty { get; set; }
        public decimal totalPrice { get; set; }
        public DateTime date { get; set; }
        public string username { get; set; }
    }
}
