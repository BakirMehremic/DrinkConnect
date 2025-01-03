using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DrinkConnect.Models;

namespace DrinkConnect.Dtos
{
    public class EditOrderDto
    {
        public float? TotalPrice { get; set; }
        
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

        public string? UserId { get; set; } 

        public ICollection<OrderProduct>? OrderProducts {get; set;}
    }
}