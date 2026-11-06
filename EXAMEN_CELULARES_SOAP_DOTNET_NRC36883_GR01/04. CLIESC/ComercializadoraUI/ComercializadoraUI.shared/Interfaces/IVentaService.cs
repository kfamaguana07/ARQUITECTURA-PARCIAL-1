using System.Collections.Generic;
using System.Threading.Tasks;
using ComercializadoraServiceReference;

namespace ComercializadoraUI.shared.Interfaces
{
    public interface IVentaService
    {
        Task<RespuestaVenta> ProcesarVentaAsync(SolicitudVentaRequest solicitud);
        Task<List<FacturaConsultaContract>> ObtenerHistorialFacturasAsync(string cedulaCliente = null);
    }
}