using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkConnect.Models
{
    public class Notification
    {
        public int Id {get; set;}

        public string? Text {get; set;}

        public int OrderId { get; set; }

        public string? UserId { get; set; } 

    }
}