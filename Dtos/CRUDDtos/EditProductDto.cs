using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DrinkConnect.Enums;

namespace DrinkConnect.Dtos
{
    public class EditProductDto
    {
        public int? Quantity { get; set; }

        public string? Name { get; set; }
       
        public string? Description { get; set; }

        public float? Price { get; set; }

        public ProductCategory? Category {get; set; }
    }
}