using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DrinkConnect.Dtos.CRUDDtos
{
    public class NewNotificationDto 
    {
        [Required]
        public required int OrderId {get; set;}

        [Required]
        public required string UserId {get; set;}

        public string? Text {get; set;}
        
    }
}