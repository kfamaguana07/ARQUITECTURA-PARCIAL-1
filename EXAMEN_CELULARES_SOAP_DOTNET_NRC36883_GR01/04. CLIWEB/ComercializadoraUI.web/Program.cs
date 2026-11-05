using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ComercializadoraUI.shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

// 1. Registrar componentes interactivos
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// 2. Registrar servicios de la capa compartida
builder.Services.AddComercializadoraSharedServices();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

// 3. Mapear componentes con modo interactivo y la asamblea compartida
app.MapRazorComponents<ComercializadoraUI.web.Components.App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(ComercializadoraUI.shared.Pages.Home).Assembly);

app.Run();