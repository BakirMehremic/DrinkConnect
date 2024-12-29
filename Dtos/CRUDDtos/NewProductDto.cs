using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkConnect.Dtos
{
    public class NewProductDto
    {
        // set category to new by default in service layer
        [Required]
        public int Quantity { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public float? Price { get; set; }

    }
}