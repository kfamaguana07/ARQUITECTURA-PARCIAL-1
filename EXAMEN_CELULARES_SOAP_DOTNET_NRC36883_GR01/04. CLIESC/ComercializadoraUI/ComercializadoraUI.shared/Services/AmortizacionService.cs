using ComercializadoraServiceReference;
using ComercializadoraUI.shared.Helpers;
using ComercializadoraUI.shared.Interfaces;

namespace ComercializadoraUI.shared.Services
{
    public class AmortizacionService : IAmortizacionService
    {
        private readonly ComercializadoraServiceClient _client;

        public AmortizacionService(ComercializadoraServiceClient client)
        {
            _client = client;
        }

        public async Task<List<CuotaAmortizacion>> ConsultarAmortizacionAsync(int codigoCredito)
        {
            try
            {
                var resultado = await _client.ConsultarAmortizacionCreditoAsync(codigoCredito);
                return resultado != null ? resultado.ToList() : new List<CuotaAmortizacion>();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ExceptionHelper.ObtenerMensajeAmigable(ex));
            }
        }
    }
}