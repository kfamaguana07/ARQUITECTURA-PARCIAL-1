using Microsoft.AspNetCore.Authentication;
using WS_ConUni_DotNet_Rest_GR5.Modelos.Servicios;
using WS_ConUni_DotNet_Rest_GR5.Modelos.Utilidades;
using WS_ConUni_DotNet_Rest_GR5.Seguridad;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCorsConfigurado();
builder.Services.AddAuthentication("BearerToken")
    .AddScheme<AuthenticationSchemeOptions, BearerTokenAuthenticationHandler>("BearerToken", _ => { });
builder.Services.AddAuthorization();

builder.Services.AddSingleton<TokenProvider>();
builder.Services.AddSingleton<AuthService>();

builder.Services.AddSingleton<ConversorLongitud>();
builder.Services.AddSingleton<ConversorTemperatura>();
builder.Services.AddSingleton<ConversorMasa>();
builder.Services.AddSingleton<ServicioConversion>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseCors(CorsExtensions.PoliticaCORS);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
