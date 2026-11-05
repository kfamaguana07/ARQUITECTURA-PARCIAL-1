using Comercializadora_ClienteConsola_Dotnet_GR01.Views;
using ComercializadoraServiceReference;
using System;

namespace Comercializadora_ClienteConsola_Dotnet_GR01.Controllers
{
    public class AmortizacionController
    {
        private readonly AmortizacionView _amortizacionView = new AmortizacionView();

        public void ConsultarTabla()
        {
            int codCredito = _amortizacionView.CapturarCodigoCredito();

            using (var client = new ComercializadoraServiceClient())
            {
                try
                {
                    var tabla = client.ConsultarAmortizacionCredito(codCredito);
                    _amortizacionView.MostrarTablaAmortizacion(tabla, codCredito);
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