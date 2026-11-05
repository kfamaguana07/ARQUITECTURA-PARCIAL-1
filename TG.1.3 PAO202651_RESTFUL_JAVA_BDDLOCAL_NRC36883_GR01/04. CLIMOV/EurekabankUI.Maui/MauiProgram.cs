using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace SistemaConversorUI.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

            // 1. Cliente HTTP
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

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}