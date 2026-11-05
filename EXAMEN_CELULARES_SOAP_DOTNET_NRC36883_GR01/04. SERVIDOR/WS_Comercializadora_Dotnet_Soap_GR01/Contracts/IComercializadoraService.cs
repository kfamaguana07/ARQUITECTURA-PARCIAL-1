using System.Collections.Generic;
using System.ServiceModel;
using WS_Comercializadora_Dotnet_Soap_GR01.BanquitoServiceReference;
using WS_Comercializadora_Dotnet_Soap_GR01.Models;

namespace WS_Comercializadora_Dotnet_Soap_GR01.Contracts
{
    [ServiceContract]
    public interface IComercializadoraService
    {
        // 1. Mantenimiento del Catálogo de Productos
        [OperationContract]
        List<Producto> ObtenerCatalogoProductos();

        [OperationContract]
        bool GuardarProducto(Producto producto);

        [OperationContract]
        bool ActualizarProducto(Producto producto);

        [OperationContract]
        bool EliminarProducto(int productoId);

        // 2. Orquestación de Ventas (Efectivo -42% / Crédito Directo con WCF BanQuito)
        [OperationContract]
        RespuestaVenta ProcesarVenta(SolicitudVentaRequest solicitud);

        // 3. Consulta de Amortización desde la Comercializadora
        [OperationContract]
        List<CuotaAmortizacion> ConsultarAmortizacionCredito(int codigoCredito);
    }
}