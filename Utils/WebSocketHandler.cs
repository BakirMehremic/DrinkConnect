using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DrinkConnect.Utils;
public class WebSocketHandler
{
    public async Task HandleWebSocketAsync(WebSocket webSocket, string userId)
    {
        var buffer = new byte[1024 * 4]; // 4 KB buffer

        Console.WriteLine($"WebSocket connection established for user: {userId}");

        while (webSocket.State == WebSocketState.Open)
        {
            try
            {
                // Receive data from the WebSocket client
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    Console.WriteLine($"WebSocket connection closed by user: {userId}");
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing connection", CancellationToken.None);
                }
                else if (result.MessageType == WebSocketMessageType.Text)
                {
                    // Decode the received message
                    var receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    Console.WriteLine($"Received from user {userId}: {receivedMessage}");

                    // Send a response back to the client
                    var responseMessage = Encoding.UTF8.GetBytes($"Hello {userId}, server received: {receivedMessage}");
                    await webSocket.SendAsync(new ArraySegment<byte>(responseMessage), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
            catch (WebSocketException ex)
            {
                Console.WriteLine($"WebSocket error for user {userId}: {ex.Message}");
                break;
            }
        }

        Console.WriteLine($"WebSocket connection for user {userId} closed.");
    }
}