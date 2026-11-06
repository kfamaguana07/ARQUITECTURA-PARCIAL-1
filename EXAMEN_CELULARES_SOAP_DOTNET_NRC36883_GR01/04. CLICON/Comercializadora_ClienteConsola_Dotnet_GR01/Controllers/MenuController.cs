using Comercializadora_ClienteConsola_Dotnet_GR01.Views;

namespace Comercializadora_ClienteConsola_Dotnet_GR01.Controllers
{
    public class MenuController
    {
        private readonly MenuView _menuView = new MenuView();
        private readonly ProductoController _productoController = new ProductoController();
        private readonly VentaController _ventaController = new VentaController();
        private readonly AmortizacionController _amortizacionController = new AmortizacionController();
        private readonly FacturaController _facturaController = new FacturaController();

        public void Iniciar()
        {
            bool salir = false;
            while (!salir)
            {
                int opcion = _menuView.MostrarMenuPrincipal();
                switch (opcion)
                {
                    case 1:
                        _productoController.EjecutarFlujo();
                        break;
                    case 2:
                        _ventaController.ProcesarVenta();
                        break;
                    case 3:
                        _amortizacionController.ConsultarTabla();
                        break;
                    case 4:
                        _facturaController.ConsultarHistorial();
                        break;
                    case 0:
                        salir = true;
                        ConsoleHelper.MostrarMensajeInfo("Gracias por utilizar el sistema de la Comercializadora.");
                        break;
                }
            }
        }
    }
}