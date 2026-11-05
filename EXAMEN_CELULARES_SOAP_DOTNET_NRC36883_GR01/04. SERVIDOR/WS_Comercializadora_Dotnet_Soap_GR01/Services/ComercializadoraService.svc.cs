using System;
using System.Collections.Generic;
using WS_Comercializadora_Dotnet_Soap_GR01.BanquitoServiceReference;
using WS_Comercializadora_Dotnet_Soap_GR01.BusinessLogic;
using WS_Comercializadora_Dotnet_Soap_GR01.Contracts;
using WS_Comercializadora_Dotnet_Soap_GR01.Models;

namespace WS_Comercializadora_Dotnet_Soap_GR01.Services
{
    public class ComercializadoraService : IComercializadoraService
    {
        private readonly ProductoBLL _productoBLL = new ProductoBLL();
        private readonly VentaBLL _ventaBLL = new VentaBLL();

        // --- 1. Mantenimiento del Catálogo de Productos ---

        public List<Producto> ObtenerCatalogoProductos()
        {
            return _productoBLL.ObtenerCatalogo();
        }

        public bool GuardarProducto(Producto producto)
        {
            if (producto == null) return false;
            return _productoBLL.Guardar(producto);
        }

        public bool ActualizarProducto(Producto producto)
        {
            if (producto == null) return false;
            return _productoBLL.Actualizar(producto);
        }

        public bool EliminarProducto(int productoId)
        {
            return _productoBLL.Eliminar(productoId);
        }

        // --- 2. Orquestación del Proceso de Venta ---

        public RespuestaVenta ProcesarVenta(SolicitudVentaRequest solicitud)
        {
            if (solicitud == null || string.IsNullOrWhiteSpace(solicitud.CedulaCliente) || solicitud.ProductoId <= 0)
            {
                return new RespuestaVenta
                {
                    Exitosa = false,
                    Mensaje = "Los datos de la solicitud de venta son incompletos o no válidos."
                };
            }

            return _ventaBLL.ProcesarVenta(solicitud);
        }

        // --- 3. Consulta de Tabla de Amortización desde la Comercializadora ---

        public List<CuotaAmortizacion> ConsultarAmortizacionCredito(int codigoCredito)
        {
            if (codigoCredito <= 0) return new List<CuotaAmortizacion>();
            return _ventaBLL.ConsultarAmortizacion(codigoCredito);
        }
    }
}