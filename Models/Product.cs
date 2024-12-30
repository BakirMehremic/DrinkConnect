using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using DrinkConnect.Dtos;
using DrinkConnect.Enums;

namespace DrinkConnect.Models
{
    [Table("Products")]
    public class Product
    {
        public  int Id {get; set;}

        public int Quantity { get; set; } = 10;

        public required string Name {get; set;}

        public string? Description { get; set; }

        public required float Price {get; set;}

        public required ProductCategory Category {get; set;}

    }
}