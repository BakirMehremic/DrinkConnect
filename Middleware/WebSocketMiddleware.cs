using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace DrinkConnect.Middleware
{
    public class WebSocketMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public WebSocketMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        // expects the jwt to be in header - access_token
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path == "/notifications" && context.WebSockets.IsWebSocketRequest)
            {
                var token = context.Request.Headers["access_token"].ToString();

                if (string.IsNullOrEmpty(token))
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Missing or invalid access token");
                    return;
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["JWT:SigningKey"]);

                try
                {
                    var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidIssuer = _configuration["JWT:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = _configuration["JWT:Audience"],
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    }, out _);

                    context.User = claimsPrincipal;

                    var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (string.IsNullOrEmpty(userId))
                    {
                        context.Response.StatusCode = 403; 
                        await context.Response.WriteAsync("User ID not found in token");
                        return;
                    }
                    var webSocket = await context.WebSockets.AcceptWebSocketAsync();

                    var webSocketHandler = context.RequestServices.GetRequiredService<Utils.WebSocketHandler>();
                    await webSocketHandler.HandleWebSocketAsync(webSocket, userId);
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync($"Invalid token: {ex.Message}");
                }
            }
            else
            {
                await _next(context);
            }
        }
    }
}