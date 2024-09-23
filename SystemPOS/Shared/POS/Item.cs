using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemPOS.Shared.POS
{
    public class reqItem
    {
        public string Username { get; set; }
    }
    public class respItem
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public int Category { get; set; }
        public decimal Price { get; set; }
        public decimal Qty { get; set; }
        public string Path { get; set; }
        public string Mime { get; set; } = string.Empty;
    }

}
