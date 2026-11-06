using System;
using System.Collections.Generic;
using System.Linq;
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
            // 1. Validar existencia del cliente
            var cliente = _clienteDAL.ObtenerPorCedula(solicitud.CedulaCliente);
            if (cliente == null)
            {
                return new RespuestaVenta
                {
                    Exitosa = false,
                    Mensaje = "El cliente no se encuentra registrado en la Comercializadora."
                };
            }

            // 2. Validar y construir la lista de detalles consultando precios reales en la BD
            if (solicitud.Items == null || !solicitud.Items.Any())
            {
                return new RespuestaVenta
                {
                    Exitosa = false,
                    Mensaje = "El carrito de compras no contiene productos."
                };
            }

            var detallesFactura = new List<FacturaDetalleItem>();
            decimal subtotalGeneral = 0m;

            foreach (var item in solicitud.Items)
            {
                if (item.Cantidad <= 0) continue;

                var prodBD = _productoDAL.ObtenerPorId(item.ProductoId);
                if (prodBD == null)
                {
                    return new RespuestaVenta
                    {
                        Exitosa = false,
                        Mensaje = $"El producto con ID #{item.ProductoId} no existe en el catálogo."
                    };
                }

                detallesFactura.Add(new FacturaDetalleItem
                {
                    ProductoId = prodBD.ProId,
                    Cantidad = item.Cantidad,
                    PrecioUnitario = prodBD.ProPrecio
                });

                subtotalGeneral += (prodBD.ProPrecio * item.Cantidad);
            }

            if (!detallesFactura.Any())
            {
                return new RespuestaVenta
                {
                    Exitosa = false,
                    Mensaje = "No hay ítems con cantidades válidas para procesar."
                };
            }

            // 3. Evaluar modalidad de pago
            if (solicitud.FormaPago.Equals("E", StringComparison.OrdinalIgnoreCase))
            {
                return ProcesarVentaEfectivo(cliente, subtotalGeneral, detallesFactura);
            }
            else if (solicitud.FormaPago.Equals("C", StringComparison.OrdinalIgnoreCase))
            {
                return ProcesarVentaCredito(cliente, subtotalGeneral, detallesFactura, solicitud);
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

        private RespuestaVenta ProcesarVentaEfectivo(ClienteComercializadora cliente, decimal subtotal, List<FacturaDetalleItem> detalles)
        {
            // Descuento del 42% en compras en efectivo
            decimal descuento = Math.Round(subtotal * 0.42m, 2);
            decimal totalPagar = subtotal - descuento;

            Factura factura = new Factura
            {
                CliId = cliente.CliId,
                FacFecha = DateTime.Now,
                FacFormaPago = "E",
                FacSubtotal = subtotal,
                FacDescuento = descuento,
                FacPrecioTotal = totalPagar,
                FacNumCuotas = null,
                FacPrecioCuota = null,
                CreId = null
            };

            int facturaId = _facturaDAL.GuardarFactura(factura, detalles);

            return new RespuestaVenta
            {
                Exitosa = true,
                Mensaje = "Venta multiproducto en efectivo completada con éxito (-42% descuento).",
                FacturaId = facturaId,
                TotalPagar = totalPagar,
                CreditoId = null,
                TablaAmortizacion = null
            };
        }

        private RespuestaVenta ProcesarVentaCredito(ClienteComercializadora cliente, decimal totalMonto, List<FacturaDetalleItem> detalles, SolicitudVentaRequest solicitud)
        {
            if (solicitud.NumeroCuotas < 3 || solicitud.NumeroCuotas > 18)
            {
                return new RespuestaVenta
                {
                    Exitosa = false,
                    Mensaje = "El plazo para crédito directo debe estar comprendido entre 3 y 18 meses."
                };
            }

            using (BanquitoServiceClient bancoClient = new BanquitoServiceClient())
            {
                try
                {
                    // 1. Validar reglas de crédito en BanQuito
                    var validacion = bancoClient.ValidarSujetoCredito(solicitud.CedulaCliente);
                    if (!validacion.EsSujeto)
                    {
                        return new RespuestaVenta
                        {
                            Exitosa = false,
                            Mensaje = "Crédito Rechazado por el Banco: " + validacion.Mensaje
                        };
                    }

                    // 2. Verificar cupo máximo autorizado contra el monto total del carrito
                    decimal montoMaximo = bancoClient.ObtenerMontoMaximo(validacion.CodigoCliente);
                    if (totalMonto > montoMaximo)
                    {
                        return new RespuestaVenta
                        {
                            Exitosa = false,
                            Mensaje = $"El monto total del carrito (${totalMonto:N2}) excede el cupo máximo otorgado por el banco (${montoMaximo:N2})."
                        };
                    }

                    // 3. Otorgar crédito en el banco
                    int codCredito = bancoClient.OtorgarCredito(validacion.CodigoCliente, totalMonto, solicitud.NumeroCuotas);
                    if (codCredito <= 0)
                    {
                        return new RespuestaVenta
                        {
                            Exitosa = false,
                            Mensaje = "No se pudo registrar el crédito en el sistema bancario."
                        };
                    }

                    // 4. Obtener tabla de amortización
                    var tablaAmortizacion = bancoClient.ConsultarTablaAmortizacion(codCredito);
                    decimal valorCuota = (tablaAmortizacion != null && tablaAmortizacion.Length > 0) ? tablaAmortizacion[0].ValorCuota : 0;

                    // 5. Registrar factura localmente
                    Factura factura = new Factura
                    {
                        CliId = cliente.CliId,
                        FacFecha = DateTime.Now,
                        FacFormaPago = "C",
                        FacSubtotal = totalMonto,
                        FacDescuento = 0m,
                        FacPrecioTotal = totalMonto,
                        FacNumCuotas = solicitud.NumeroCuotas,
                        FacPrecioCuota = valorCuota,
                        CreId = codCredito
                    };

                    int facturaId = _facturaDAL.GuardarFactura(factura, detalles);

                    return new RespuestaVenta
                    {
                        Exitosa = true,
                        Mensaje = "Crédito aprobado y venta multiproducto registrada correctamente.",
                        FacturaId = facturaId,
                        TotalPagar = totalMonto,
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

        public List<FacturaConsultaContract> ObtenerHistorialFacturas(string cedulaCliente)
        {
            string cedulaLimpia = string.IsNullOrWhiteSpace(cedulaCliente) ? null : cedulaCliente.Trim();
            return _facturaDAL.ObtenerHistorialFacturas(cedulaLimpia);
        }
    }
}