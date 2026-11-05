using ComercializadoraServiceReference;
using ComercializadoraUI.shared.Helpers;
using ComercializadoraUI.shared.Interfaces;

namespace ComercializadoraUI.shared.Services
{
    public class VentaService : IVentaService
    {
        private readonly ComercializadoraServiceClient _client;

        public VentaService(ComercializadoraServiceClient client)
        {
            _client = client;
        }

        public async Task<RespuestaVenta> ProcesarVentaAsync(SolicitudVentaRequest solicitud)
        {
            try
            {
                return await _client.ProcesarVentaAsync(solicitud);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ExceptionHelper.ObtenerMensajeAmigable(ex));
            }
        }
    }
}