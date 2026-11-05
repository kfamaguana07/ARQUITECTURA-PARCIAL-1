using ComercializadoraServiceReference;

namespace ComercializadoraUI.shared.Interfaces
{
    public interface IVentaService
    {
        Task<RespuestaVenta> ProcesarVentaAsync(SolicitudVentaRequest solicitud);
    }
}