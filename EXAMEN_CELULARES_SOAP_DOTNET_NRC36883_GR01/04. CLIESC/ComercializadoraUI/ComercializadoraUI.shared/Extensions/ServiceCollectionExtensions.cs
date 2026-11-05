using ComercializadoraServiceReference;
using ComercializadoraUI.shared.Interfaces;
using ComercializadoraUI.shared.Services;
using Microsoft.Extensions.DependencyInjection;
using System.ServiceModel;

namespace ComercializadoraUI.shared.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddComercializadoraSharedServices(this IServiceCollection services)
        {
            string ipServidor = "10.96.24.79";
            string puerto = "9092";

            string wcfEndpointUrl = $"http://{ipServidor}:{puerto}/Services/ComercializadoraService.svc";

            // Registro del Cliente WCF proxy especificando la URL por IP
            services.AddScoped<ComercializadoraServiceClient>(sp =>
            {
                var binding = new BasicHttpBinding();
                var endpoint = new EndpointAddress(wcfEndpointUrl);
                return new ComercializadoraServiceClient(binding, endpoint);
            });
            services.AddScoped<IProductoService, ProductoService>();
            services.AddScoped<IVentaService, VentaService>();
            services.AddScoped<IAmortizacionService, AmortizacionService>();
            return services;
        }
    }
}