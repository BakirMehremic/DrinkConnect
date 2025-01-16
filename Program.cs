using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using DrinkConnect.Data;
using DrinkConnect.Interfaces.RepositoryInterfaces;
using DrinkConnect.Interfaces.ServiceInterfaces;
using DrinkConnect.Models;
using DrinkConnect.Repositories;
using DrinkConnect.Services;
using DrinkConnect.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure DbContext with MySQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new Exception("Check connection string");
    options.UseMySQL(connectionString);
});

// Configure Identity with custom user and role
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
    options.DefaultChallengeScheme =
    options.DefaultForbidScheme =
    options.DefaultScheme =
    options.DefaultSignInScheme =
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])
        ),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromSeconds(30)
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("WaiterOnly", policy => policy.RequireRole("Waiter"));
    options.AddPolicy("BartenderOnly", policy => policy.RequireRole("Bartender"));
});


builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IBartenderService, BartenderService>();
builder.Services.AddScoped<IBartenderRepository, BartenderRepository>();
builder.Services.AddScoped<IWaiterRepsoritoy, WaiterRepository>();
builder.Services.AddScoped<IWaiterService, WaiterService>();
//builder.Services.AddScoped<IWebsocketService, WebSocketService>();
//builder.Services.AddScoped<IWebSocketRepository, WebSocketRepository>();

builder.Services.AddScoped<WebSocketHandler>();

// injected because UserUtils.GetCurrentUserId() cant be static 
builder.Services.AddScoped<UserUtils>();

// same reason as UserUtils
builder.Services.AddScoped<TokenUtils>();


builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    });



builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});


var app = builder.Build();

// Seed roles and users during startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Seed roles
        await RoleSeeder.SeedRolesAsync(services);

        // Seed a default admin
        await RoleSeeder.SeedDefaultAdminAsync(services);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while seeding roles or users: {ex.Message}");
    }
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();



app.MapControllers();

// Enable WebSocket middleware
app.UseWebSockets(new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromSeconds(120) // Adjust as needed
});

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/notifications" && context.WebSockets.IsWebSocketRequest)
    {
        // Extract the token from the query string
        var token = context.Request.Query["access_token"].ToString();

        if (string.IsNullOrEmpty(token))
        {
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("Missing or invalid access token");
            return;
        }

        // Validate the token
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"]); // Replace with your signing key
        try
        {
            var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = builder.Configuration["JWT:Issuer"],
                ValidateAudience = true,
                ValidAudience = builder.Configuration["JWT:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out _);

            // Set the user in the HttpContext
            context.User = claimsPrincipal;

            // Get the user ID
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                context.Response.StatusCode = 403; // Forbidden
                await context.Response.WriteAsync("User ID not found in token");
                return;
            }

            // Accept the WebSocket connection
            var webSocket = await context.WebSockets.AcceptWebSocketAsync();

            // Pass the user ID to your WebSocketHandler
            var webSocketHandler = context.RequestServices.GetRequiredService<DrinkConnect.Utils.WebSocketHandler>();
            await webSocketHandler.HandleWebSocketAsync(webSocket, userId);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync($"Invalid token: {ex.Message}");
        }
    }
    else
    {
        await next();
    }
});


app.Run();
