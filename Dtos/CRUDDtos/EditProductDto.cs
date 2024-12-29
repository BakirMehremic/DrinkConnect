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
        [Required]
        public int? Quantity { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public float? Price { get; set; }

        [Required]
        public ProductCategory Category {get; set; }
    }
}