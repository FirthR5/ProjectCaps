﻿using System.ComponentModel.DataAnnotations;

namespace Caps_Project.DTOs.OrdenDTOs
{
    public class ProductCarritoDTO
    {
        [Key]
        public int IdItem { get; set; }

        public string ProductName { get; set; }

        public decimal UnitPrice { get; set; }


        public int Quantity { get; set; }

        public int ProductPriceID { get; set; }
        public int ProductId { get; set; }
    }
}
