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


builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;

    options.SignIn.RequireConfirmedEmail = true; 
    // to make the code shorter
    options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
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
builder.Services.AddScoped<IWebSocketService, WebSocketService>();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("Mailtrap"));
builder.Services.AddTransient<EmailService>();

// singleton because one instance is needed to keep a dictionary of connected sockets
builder.Services.AddSingleton<WebSocketHandler>();

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

// Apply migrations on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();  
}

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

// enabling websockets
app.UseWebSockets(new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromSeconds(120) 
});

// used for authentification of websocket requests
app.UseMiddleware<DrinkConnect.Middleware.WebSocketMiddleware>();


app.Run();
