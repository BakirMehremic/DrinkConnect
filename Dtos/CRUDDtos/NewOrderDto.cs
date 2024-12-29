using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DrinkConnect.Models;
using Microsoft.AspNetCore.Mvc;

namespace DrinkConnect.Dtos
{
    public class NewOrderDto
    {
        public float? TotalPrice { get; set; }
        
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public string? UserId { get; set; } 

        [Required]
        public ICollection<OrderProduct>? OrderProducts {get; set;}
    }
}