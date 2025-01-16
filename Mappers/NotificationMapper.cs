using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrinkConnect.Dtos.CRUDDtos;
using DrinkConnect.Models;

namespace DrinkConnect.Mappers
{
    public static class NotificationMapper
    {
        public static Notification NewNotificationDtoToNotification(this NewNotificationDto dto){
            return new Notification{
                OrderId=dto.OrderId,
                UserId=dto.UserId,
                Text=dto.Text
            };
        }
    }
}