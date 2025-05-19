using ApiPruebaVive.Context;
using ApiPruebaVive.Middleware;
using ApiPruebaVive.Models;
using ApiPruebaVive.Services;
using ApiPruebaVive.WebSockets;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor 
builder.Services.AddSingleton<CustomWebSocketManager>();
builder.Services.AddSingleton<WebSocketHandler>();

// Configurar la cadena de conexión a PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Configurar el DbContext para usar PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString)); // Usa Npgsql para PostgreSQL

// Registrar TelnetConnectionManager como singleton
builder.Services.AddScoped<TelnetConnectionManager>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("WebApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "https://ca.adsodev.com")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Configurar autenticación JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Agregar servicios al contenedor
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();

// Crear la aplicación
var app = builder.Build();

app.UseWebSockets(new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromSeconds(120)
});


// Verificar la conexión a la base de datos
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        dbContext.Database.OpenConnection();
        Console.WriteLine("¡Conexión exitosa a la base de datos!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al conectar a la base de datos: {ex.Message}");
    }
}

// Inicializar conexiones Telnet solo para OLTs "ZTEC600"
using (var scope = app.Services.CreateScope())
{
    var telnetManager = scope.ServiceProvider.GetRequiredService<TelnetConnectionManager>();
    await telnetManager.InitializeConnectionsAsync();
}

// Configurar el pipeline de solicitud HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseMiddleware<WebSocketMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();

// Configurar CORS
app.UseCors("WebApp");

app.MapControllers();

app.Run();
