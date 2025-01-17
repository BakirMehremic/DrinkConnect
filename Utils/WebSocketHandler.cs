using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DrinkConnect.Dtos.CRUDDtos;

namespace DrinkConnect.Utils
{
    public class WebSocketHandler
    {
        private readonly ConcurrentDictionary<string, WebSocket> _connectedSockets = new ConcurrentDictionary<string, WebSocket>();

        public async Task HandleWebSocketAsync(WebSocket webSocket, string userId)
        {
            if (_connectedSockets.TryAdd(userId, webSocket))
            {
                Console.WriteLine($"WebSocket connection established for user: {userId}");
            }
            else
            {
                Console.WriteLine($"Failed to add WebSocket for user: {userId}");
                return;
            }

            var buffer = new byte[1024 * 4];

            try
            {
                while (webSocket.State == WebSocketState.Open)
                {
                    var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing connection", CancellationToken.None);
                    }
                    else if (result.MessageType == WebSocketMessageType.Text)
                    {
                        var receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        var responseMessage = Encoding.UTF8.GetBytes($"Hello {userId}, server received: {receivedMessage}");
                        Console.WriteLine($"Received: {receivedMessage}");
                        await webSocket.SendAsync(new ArraySegment<byte>(responseMessage), WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                }
            }
            catch (WebSocketException ex)
            {
                Console.WriteLine($"WebSocket error for user {userId}: {ex.Message}");
            }
            finally
            {
                if (_connectedSockets.TryRemove(userId, out _))
                {
                    Console.WriteLine($"WebSocket connection for user {userId} closed.");
                }
            }
        }

        public async Task SendNotificationAsync(string id, NewNotificationDto notification)
{
    var message = JsonSerializer.Serialize(notification);
    var bytes = Encoding.UTF8.GetBytes(message);

    if (_connectedSockets.TryGetValue(id, out var webSocket))
    {
        try
        {
            if (webSocket.State == WebSocketState.Open)
            {
                Console.WriteLine($"Sending notification to WebSocket for user: {id}");
                await webSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
            }
            else
            {
                Console.WriteLine($"WebSocket for user {id} is not open.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending notification to user {id}: {ex.Message}");
        }
    }
    else
    {
        Console.WriteLine($"No WebSocket connection found for user: {id}");
    }
}


    }
}
