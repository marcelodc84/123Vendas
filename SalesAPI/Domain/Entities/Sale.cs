using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Sale
    {
        [Key]
        public int SaleNumber { get; set; }
        public DateTime SaleDate { get; set; } = DateTime.Now;
        public string Customer { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string Branch { get; set; } = string.Empty;
        public List<SaleItem> Items { get; set; } = new List<SaleItem>();
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
