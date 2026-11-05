using ComercializadoraServiceReference;

namespace Comercializadora_ClienteConsola_Dotnet_GR01.Views
{
    public class VentaView
    {
        public SolicitudVentaRequest CapturarSolicitudVenta()
        {
            ConsoleHelper.MostrarTitulo("PROCESAR VENTA DE TELÉFONO CELULAR");

            string cedula = ConsoleHelper.LeerTextoRequerido("Número de Cédula del Cliente");
            int productoId = ConsoleHelper.LeerEntero("ID del Producto/Teléfono a Comprar", 1);

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
                ProductoId = productoId,
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