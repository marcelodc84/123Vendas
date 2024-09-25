using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Sale
    {
        public int SaleNumber { get; set; }
        public DateTime SaleDate { get; set; }
        public string Customer { get; set; }
        public decimal TotalAmount { get; set; }
        public string Branch { get; set; }
        public List<SaleItem> Items { get; set; }
        public bool IsCancelled { get; set; }
    }
}

/*
{
    "SaleNumber": 123,
    "SaleDate": "2024-09-25",
    "Customer": "qweqweqeqw",
    "TotalAmount": 123234,
    "Branch": "qweqwe",
    "Items": {},
    "IsCancelled": false
}
*/
