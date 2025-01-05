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
        
        [Required]
        public required ICollection<EditOrderProductDto> OrderProducts {get; set;}
    }
}