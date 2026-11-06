using ComercializadoraServiceReference;
using System;
using System.Collections.Generic;

namespace Comercializadora_ClienteConsola_Dotnet_GR01.Views
{
    public class VentaView
    {
        public SolicitudVentaRequest CapturarSolicitudVenta()
        {
            ConsoleHelper.MostrarTitulo("PROCESAR VENTA MULTIPRODUCTO");

            string cedula = ConsoleHelper.LeerTextoRequerido("Número de Cédula del Cliente");

            // Carrito de compras
            List<DetalleVentaItemContract> items = new List<DetalleVentaItemContract>();
            bool agregarMas = true;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n--- AGREGAR PRODUCTOS AL CARRITO ---");
            Console.ResetColor();

            while (agregarMas)
            {
                int productoId = ConsoleHelper.LeerEntero("ID del Producto a Comprar", 1);
                int cantidad = ConsoleHelper.LeerEntero("Cantidad a Comprar", 1, 100);

                items.Add(new DetalleVentaItemContract
                {
                    ProductoId = productoId,
                    Cantidad = cantidad
                });

                Console.Write("\n¿Desea agregar otro producto al carrito? (S/N): ");
                string resp = Console.ReadLine()?.Trim().ToUpper();
                agregarMas = resp == "S";
            }

            Console.WriteLine("\nModalidad de Pago:");
            Console.WriteLine("  [E] Efectivo (Aplica 42% de descuento directo)");
            Console.WriteLine("  [C] Crédito Directo (Sujeto a aprobación WCF BanQuito)");

            string formaPago = "";
            while (formaPago != "E" && formaPago != "C")
            {
                Console.Write("Seleccione Forma de Pago (E/C): ");
                formaPago = Console.ReadLine()?.Trim().ToUpper();
            }

            int cuotas = 0;
            if (formaPago == "C")
            {
                cuotas = ConsoleHelper.LeerEntero("Plazo en Meses (Cuotas entre 3 y 18)", 3, 18);
            }

            return new SolicitudVentaRequest
            {
                CedulaCliente = cedula,
                Items = items, // Asignación directa como List<DetalleVentaItemContract>    
                FormaPago = formaPago,
                NumeroCuotas = cuotas
            };
        }

        public void MostrarResultadoVenta(RespuestaVenta respuesta)
        {
            if (!respuesta.Exitosa)
            {
                ConsoleHelper.MostrarMensajeError(respuesta.Mensaje);
                return;
            }

            ConsoleHelper.MostrarMensajeExito(respuesta.Mensaje);
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine($"NÚMERO DE FACTURA GENERADA : {respuesta.FacturaId}");
            Console.WriteLine($"MONTO TOTAL PROCESADO     : ${respuesta.TotalPagar:N2}");

            if (respuesta.CreditoId.HasValue && respuesta.CreditoId.Value > 0)
            {
                Console.WriteLine($"CÓDIGO DE CRÉDITO BANQUITO : {respuesta.CreditoId.Value}");
            }
            Console.WriteLine("--------------------------------------------------------------------------------");
        }
    }
}