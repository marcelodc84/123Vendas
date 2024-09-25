using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class SaleItem
    {
        public string Product { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalItemAmount { get; set; }
    }
}

/*
{
    "Product": "qeqwe",
    "Quantity": 123,
    "UnitPrice": 654,
    "Discount": 54,
    "TotalItemAmount": 54
}
*/