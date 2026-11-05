using Microsoft.EntityFrameworkCore;
using WS_Eurekabank_Dotnet_Rest_GR01.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurar Entity Framework Core con SQL Server
builder.Services.AddDbContext<EurekaBankContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EurekaBankDB")));

// 2. Habilitar controladores de la API
builder.Services.AddControllers();

// 3. Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 4. Configurar Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurar el pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();