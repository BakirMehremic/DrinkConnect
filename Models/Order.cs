using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using DrinkConnect.Enums;

namespace DrinkConnect.Models
{
    [Table("Orders")]
    public class Order
    {
        public int Id { get; set; }

        public OrderStatus? Status { get; set; } = OrderStatus.New;

        public float? TotalPrice { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
        public DateTime? UpdatedAt { get; set; } 

        public string? UserId { get; set; } 

        [ForeignKey("UserId")]
        public User? User { get; set; }

        public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
    }
}