using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkConnect.Models
{
    [Table("OrderProducts")]
    public class OrderProduct
    {
        public int Id { get; set; }

        public int OrderId { get; set; }


        [ForeignKey("OrderId")]
        public Order? Order { get; set; }


        [ForeignKey("ProductId")]
        public required Product Product {get; set;}
        
        public int ProductId {get; set;}

        public int Quantity { get; set; }

}
}