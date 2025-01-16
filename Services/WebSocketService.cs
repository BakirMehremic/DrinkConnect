using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using DrinkConnect.Dtos.CRUDDtos;
using DrinkConnect.Interfaces.RepositoryInterfaces;
using DrinkConnect.Interfaces.ServiceInterfaces;
using DrinkConnect.Mappers;
using DrinkConnect.Models;
using DrinkConnect.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace DrinkConnect.Services
{   
    [Authorize(Policy = "WaiterOnly")]
    public class WebSocketService : IWebsocketService
    {
        private readonly IWebSocketRepository _webSocketRepository;
        public WebSocketService(
            IWebSocketRepository webSocketRepository)
        {
            _webSocketRepository = webSocketRepository;
        }

        public async Task NotifyWaiterAsync(string UserId, int OrderId,
            string Message){

            var notification = new Notification{
                UserId = UserId,
                OrderId = OrderId,
                Text = $"Order with id: {OrderId} was updated" + Message
            };

            /*await _webSocketHub.Clients.Group(UserId).SendAsync("ReceiveNotification", new
            {
                Notification = notification
            });*/
        }
    }
}