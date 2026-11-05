using System;
using System.Collections.Generic;
using WS_Comercializadora_Dotnet_Soap_GR01.BanquitoServiceReference;
using WS_Comercializadora_Dotnet_Soap_GR01.DataAccess;
using WS_Comercializadora_Dotnet_Soap_GR01.Models;

namespace WS_Comercializadora_Dotnet_Soap_GR01.BusinessLogic
{
    public class VentaBLL
    {
        private readonly ClienteDAL _clienteDAL = new ClienteDAL();
        private readonly ProductoDAL _productoDAL = new ProductoDAL();
        private readonly FacturaDAL _facturaDAL = new FacturaDAL();

        public RespuestaVenta ProcesarVenta(SolicitudVentaRequest solicitud)
        {
            // 1. Validar existencia del cliente en la Comercializadora
            var cliente = _clienteDAL.ObtenerPorCedula(solicitud.CedulaCliente);
            if (cliente == null)
            {
                return new RespuestaVenta
                {
                    Exitosa = false,
                    Mensaje = "El cliente no se encuentra registrado en la Comercializadora."
                };
            }

            // 2. Validar existencia del producto
            var producto = _productoDAL.ObtenerPorId(solicitud.ProductoId);
            if (producto == null)
            {
                return new RespuestaVenta
                {
                    Exitosa = false,
                    Mensaje = "El producto seleccionado no existe en el catálogo."
                };
            }

            // 3. Evaluar modalidad de pago
            if (solicitud.FormaPago.Equals("E", StringComparison.OrdinalIgnoreCase))
            {
                return ProcesarVentaEfectivo(cliente, producto);
            }
            else if (solicitud.FormaPago.Equals("C", StringComparison.OrdinalIgnoreCase))
            {
                return ProcesarVentaCredito(cliente, producto, solicitud);
            }
            else
            {
                return new RespuestaVenta
                {
                    Exitosa = false,
                    Mensaje = "Forma de pago inválida. Use 'E' para Efectivo o 'C' para Crédito Directo."
                };
            }
        }

        private RespuestaVenta ProcesarVentaEfectivo(ClienteComercializadora cliente, Producto producto)
        {
            // Descuento del 42% en compras en efectivo
            decimal precioConDescuento = Math.Round(producto.ProPrecio * 0.58m, 2);

            Factura factura = new Factura
            {
                CliId = cliente.CliId,
                FacFecha = DateTime.Now,
                FacFormaPago = "E",
                FacPrecioTotal = precioConDescuento,
                FacNumCuotas = null,
                FacPrecioCuota = null
            };

            int facturaId = _facturaDAL.GuardarFactura(factura, producto.ProId);

            return new RespuestaVenta
            {
                Exitosa = true,
                Mensaje = "Venta en efectivo completada con éxito. Se aplicó un 42% de descuento.",
                FacturaId = facturaId,
                TotalPagar = precioConDescuento,
                CreditoId = null,
                TablaAmortizacion = null
            };
        }

        private RespuestaVenta ProcesarVentaCredito(ClienteComercializadora cliente, Producto producto, SolicitudVentaRequest solicitud)
        {
            if (solicitud.NumeroCuotas < 3 || solicitud.NumeroCuotas > 18)
            {
                return new RespuestaVenta
                {
                    Exitosa = false,
                    Mensaje = "El plazo para crédito directo debe estar comprendido entre 3 y 18 meses."
                };
            }

            // Consumo WCF Servidor a Servidor (Comercializadora -> Banco)
            using (BanquitoServiceClient bancoClient = new BanquitoServiceClient())
            {
                try
                {
                    // Paso 1: Validar reglas de sujeto de crédito en BanQuito
                    var validacion = bancoClient.ValidarSujetoCredito(solicitud.CedulaCliente);
                    if (!validacion.EsSujeto)
                    {
                        return new RespuestaVenta
                        {
                            Exitosa = false,
                            Mensaje = "Crédito Rechazado por el Banco: " + validacion.Mensaje
                        };
                    }

                    // Paso 2: Verificar cupo máximo autorizado
                    decimal montoMaximo = bancoClient.ObtenerMontoMaximo(validacion.CodigoCliente);
                    if (producto.ProPrecio > montoMaximo)
                    {
                        return new RespuestaVenta
                        {
                            Exitosa = false,
                            Mensaje = $"El valor del teléfono (${producto.ProPrecio}) excede el cupo máximo otorgado por el banco (${montoMaximo})."
                        };
                    }

                    // Paso 3: Otorgar crédito en el banco
                    int codCredito = bancoClient.OtorgarCredito(validacion.CodigoCliente, producto.ProPrecio, solicitud.NumeroCuotas);
                    if (codCredito <= 0)
                    {
                        return new RespuestaVenta
                        {
                            Exitosa = false,
                            Mensaje = "No se pudo registrar el crédito en el sistema bancario."
                        };
                    }

                    // Paso 4: Obtener tabla de amortización generada
                    var tablaAmortizacion = bancoClient.ConsultarTablaAmortizacion(codCredito);
                    decimal valorCuota = tablaAmortizacion.Length > 0 ? tablaAmortizacion[0].ValorCuota : 0;

                    // Paso 5: Registrar factura localmente
                    Factura factura = new Factura
                    {
                        CliId = cliente.CliId,
                        FacFecha = DateTime.Now,
                        FacFormaPago = "C",
                        FacPrecioTotal = producto.ProPrecio,
                        FacNumCuotas = solicitud.NumeroCuotas,
                        FacPrecioCuota = valorCuota
                    };

                    int facturaId = _facturaDAL.GuardarFactura(factura, producto.ProId);

                    return new RespuestaVenta
                    {
                        Exitosa = true,
                        Mensaje = "Crédito aprobado y venta registrada correctamente.",
                        FacturaId = facturaId,
                        TotalPagar = producto.ProPrecio,
                        CreditoId = codCredito,
                        TablaAmortizacion = new List<CuotaAmortizacion>(tablaAmortizacion)
                    };
                }
                catch (Exception ex)
                {
                    return new RespuestaVenta
                    {
                        Exitosa = false,
                        Mensaje = "Error de comunicación con el WCF de BanQuito: " + ex.Message
                    };
                }
            }
        }

        public List<CuotaAmortizacion> ConsultarAmortizacion(int codCredito)
        {
            using (BanquitoServiceClient bancoClient = new BanquitoServiceClient())
            {
                var resultado = bancoClient.ConsultarTablaAmortizacion(codCredito);
                return resultado != null ? new List<CuotaAmortizacion>(resultado) : new List<CuotaAmortizacion>();
            }
        }
    }
}