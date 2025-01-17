using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrinkConnect.Dtos.CRUDDtos;
using DrinkConnect.Interfaces.ServiceInterfaces;
using DrinkConnect.Models;
using DrinkConnect.Utils;

namespace DrinkConnect.Services
{
    public class WebSocketService : IWebSocketService
    {
        private readonly WebSocketHandler _webSocketHandler;

        public WebSocketService(WebSocketHandler webSocketHandler)
        {
            _webSocketHandler = webSocketHandler;
        }

        public async Task SendNotificationAsync(Order order, string message)
        {
            var newNotification = new NewNotificationDto{
                UserId = order.UserId,
                OrderId = order.Id,
                Text = message
            };
            await _webSocketHandler.SendNotificationAsync(order.UserId, newNotification);
        }
        
    }
}