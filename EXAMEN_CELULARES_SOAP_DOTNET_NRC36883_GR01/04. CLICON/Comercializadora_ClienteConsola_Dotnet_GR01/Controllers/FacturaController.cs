using Comercializadora_ClienteConsola_Dotnet_GR01.Views;
using ComercializadoraServiceReference;
using System;

namespace Comercializadora_ClienteConsola_Dotnet_GR01.Controllers
{
    public class FacturaController
    {
        private readonly FacturaView _facturaView = new FacturaView();

        public void ConsultarHistorial()
        {
            string cedula = _facturaView.CapturarCedulaFiltro();

            using (var client = new ComercializadoraServiceClient())
            {
                try
                {
                    var facturas = client.ObtenerHistorialFacturas(cedula);
                    _facturaView.MostrarHistorialFacturas(facturas);
                }
                catch (Exception ex)
                {
                    ConsoleHelper.MostrarMensajeError("Error de comunicación WCF: " + ex.Message);
                }
            }

            ConsoleHelper.Pausar();
        }
    }
}