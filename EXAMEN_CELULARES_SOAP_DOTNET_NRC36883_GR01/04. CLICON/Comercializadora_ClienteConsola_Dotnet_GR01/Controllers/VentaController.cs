using Comercializadora_ClienteConsola_Dotnet_GR01.Views;
using ComercializadoraServiceReference;

namespace Comercializadora_ClienteConsola_Dotnet_GR01.Controllers
{
    public class VentaController
    {
        private readonly VentaView _ventaView = new VentaView();
        private readonly ProductoController _productoController = new ProductoController();
        private readonly AmortizacionView _amortizacionView = new AmortizacionView();

        public void ProcesarVenta()
        {
            // Muestra primero el catálogo para que el cliente elija el ID
            _productoController.ListarProductos();

            SolicitudVentaRequest solicitud = _ventaView.CapturarSolicitudVenta();

            using (var client = new ComercializadoraServiceClient())
            {
                try
                {
                    RespuestaVenta respuesta = client.ProcesarVenta(solicitud);
                    _ventaView.MostrarResultadoVenta(respuesta);

                    if (respuesta.Exitosa && respuesta.TablaAmortizacion != null && respuesta.TablaAmortizacion.Count > 0)
                    {
                        Console.WriteLine("\nDesplegando Tabla de Amortización aprobada por BanQuito...");
                        _amortizacionView.MostrarTablaAmortizacion(respuesta.TablaAmortizacion, respuesta.CreditoId ?? 0);
                    }
                }
                catch (Exception ex)
                {
                    ConsoleHelper.MostrarMensajeError("Error al procesar la venta vía SOAP: " + ex.Message);
                }
            }

            ConsoleHelper.Pausar();
        }
    }
}