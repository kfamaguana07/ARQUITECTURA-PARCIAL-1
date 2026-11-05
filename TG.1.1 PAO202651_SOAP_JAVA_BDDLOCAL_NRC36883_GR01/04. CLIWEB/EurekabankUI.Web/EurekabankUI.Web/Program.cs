using ec.edu.monster.Components;

var builder = WebApplication.CreateBuilder(args);

// 1. Servicios base de Blazor y Seguridad
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddAntiforgery();
builder.Services.AddHttpClient();

// 2. SERVICIOS CORE 
builder.Services.AddScoped<ec.edu.monster.Servicios.EstadoAplicacion>();
builder.Services.AddScoped<ec.edu.monster.Servicios.FabricaEstrategiaBackend>();

// 3. ADAPTADORES
builder.Services.AddScoped<ec.edu.monster.Interfaces.IAdaptadorAutenticacion, ec.edu.monster.Adaptadores.AdaptadorAutenticacionJavaSoap>();
builder.Services.AddScoped<ec.edu.monster.Interfaces.IAdaptadorBanco, ec.edu.monster.Adaptadores.AdaptadorBancoJavaSoap>();
builder.Services.AddScoped<ec.edu.monster.Interfaces.IAdaptadorAutenticacion, ec.edu.monster.Adaptadores.AdaptadorAutenticacionDotNetSoap>();
builder.Services.AddScoped<ec.edu.monster.Interfaces.IAdaptadorBanco, ec.edu.monster.Adaptadores.AdaptadorBancoDotNetSoap>();
builder.Services.AddScoped<ec.edu.monster.Interfaces.IAdaptadorAutenticacion, ec.edu.monster.Adaptadores.AdaptadorAutenticacionJavaRest>();
builder.Services.AddScoped<ec.edu.monster.Interfaces.IAdaptadorBanco, ec.edu.monster.Adaptadores.AdaptadorBancoJavaRest>();
builder.Services.AddScoped<ec.edu.monster.Interfaces.IAdaptadorAutenticacion, ec.edu.monster.Adaptadores.AdaptadorAutenticacionDotNetRest>();
builder.Services.AddScoped<ec.edu.monster.Interfaces.IAdaptadorBanco, ec.edu.monster.Adaptadores.AdaptadorBancoDotNetRest>();

// --- FIN DE LOS SERVICIOS ---
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(ec.edu.monster.Vistas.MainLayout).Assembly);

app.Run();