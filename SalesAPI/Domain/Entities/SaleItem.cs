﻿using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class SaleItem
    {
        [Key]
        public string Product { get; set; } = string.Empty;
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