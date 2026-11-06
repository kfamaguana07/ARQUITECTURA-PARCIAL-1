using ComercializadoraServiceReference;
using System;
using System.Collections.Generic;

namespace Comercializadora_ClienteConsola_Dotnet_GR01.Views
{
    public class FacturaView
    {
        public string CapturarCedulaFiltro()
        {
            ConsoleHelper.MostrarTitulo("CONSULTAR HISTORIAL DE FACTURAS");
            Console.Write("Ingrese Cédula del Cliente (Oprima [ENTER] para ver todas): ");
            return Console.ReadLine()?.Trim();
        }

        public void MostrarHistorialFacturas(IEnumerable<FacturaConsultaContract> facturas)
        {
            ConsoleHelper.MostrarTitulo("HISTORIAL DE FACTURAS REGISTRADAS");

            int contador = 0;
            foreach (var f in facturas)
            {
                contador++;
                string tipoPago = f.FormaPago == "E" ? "EFECTIVO (-42%)" : $"CRÉDITO DIRECTO #{f.CreditoId}";

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\n[FACTURA #{f.FacturaId}] - Fecha: {f.FechaVenta:dd/MM/yyyy HH:mm} | Cédula: {f.CedulaCliente} | Pago: {tipoPago}");
                Console.ResetColor();
                Console.WriteLine(new string('-', 80));
                Console.WriteLine(string.Format("{0,-6} | {1,-35} | {2,-8} | {3,-12} | {4,-12}", "ID", "PRODUCTO", "CANT", "PRECIO U.", "SUBTOTAL"));
                Console.WriteLine(new string('-', 80));

                if (f.Detalle != null)
                {
                    foreach (var det in f.Detalle)
                    {
                        Console.WriteLine(string.Format("{0,-6} | {1,-35} | {2,-8} | ${3,-11:N2} | ${4,-11:N2}",
                            det.ProductoId,
                            det.ProductoNombre.Length > 33 ? det.ProductoNombre.Substring(0, 30) + "..." : det.ProductoNombre,
                            det.Cantidad,
                            det.PrecioUnitario,
                            det.Subtotal));
                    }
                }

                Console.WriteLine(new string('-', 80));
                Console.WriteLine(string.Format("{0,63} Subtotal : ${1,10:N2}", "", f.Subtotal));
                if (f.Descuento > 0)
                {
                    Console.WriteLine(string.Format("{0,63} Descuento: -${1,9:N2}", "", f.Descuento));
                }
                Console.WriteLine(string.Format("{0,63} TOTAL    : ${1,10:N2}", "", f.Total));
            }

            if (contador == 0)
            {
                ConsoleHelper.MostrarMensajeInfo("No se encontraron facturas registradas en el sistema.");
            }
        }
    }
}