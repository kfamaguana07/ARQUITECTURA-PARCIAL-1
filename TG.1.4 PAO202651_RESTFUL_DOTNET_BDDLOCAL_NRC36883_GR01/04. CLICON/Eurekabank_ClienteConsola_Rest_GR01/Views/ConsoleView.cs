using Eurekabank_ClienteConsola_Rest_GR01.Models;

namespace Eurekabank_ClienteConsola_Rest_GR01.Views
{
    public class ConsoleView
    {
        public void ShowWelcome()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==============================");
            Console.WriteLine("Cliente Consola EuBank - REST");
            Console.WriteLine("Sistema Bancario Avanzado");
            Console.WriteLine("==============================");
            Console.ResetColor();
        }

        public (string?, string?) GetLoginCredentials()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Usuario: ");
            Console.ResetColor();
            string? usuario = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Contraseña: ");
            Console.ResetColor();
            string? clave = Console.ReadLine();

            return (usuario, clave);
        }

        public void ShowLoginSuccess(string user)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nBienvenido, {user}!");
            Console.ResetColor();
        }

        public void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error: {message}");
            Console.ResetColor();
        }

        public int ShowDashboardMenu()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n=== Dashboard REST ===");
            Console.WriteLine("1. Ver Cuentas");
            Console.WriteLine("2. Ver Movimientos");
            Console.WriteLine("3. Crear Movimiento");
            Console.WriteLine("4. Salir");
            Console.ResetColor();
            Console.Write("Opcion: ");
            string? input = Console.ReadLine();
            return int.TryParse(input, out int opcion) ? opcion : 0;
        }

        public void DisplayCuentas(List<Cuenta> cuentas)
        {
            if (cuentas.Any())
            {
                Console.WriteLine("\n--- Lista de Cuentas ---");
                string separator = new string('-', 100);
                Console.WriteLine(separator);
                Console.WriteLine("Codigo   | Cliente                      | Moneda | Saldo    | Estado");
                Console.WriteLine(separator);
                foreach (var c in cuentas)
                {
                    Console.WriteLine($"{c.Codigo,-7} | {c.NombreCliente,-28} | {c.Moneda,-6} | {c.Saldo,8:F2} | {c.Estado,-7}");
                }
                Console.WriteLine(separator);
            }
            else
            {
                Console.WriteLine("No hay cuentas disponibles.");
            }
        }

        public string? GetMovimientoFilter()
        {
            Console.Write("\nFiltrar por cuenta (dejar vacío para todas): ");
            return Console.ReadLine();
        }

        public void DisplayMovimientos(List<Movement> movimientos)
        {
            if (movimientos.Any())
            {
                Console.WriteLine("\n--- Lista de Movimientos ---");
                string separator = new string('-', 110);
                Console.WriteLine(separator);
                Console.WriteLine("Cuenta                       | Numero | Fecha      | Tipo           | Importe  | Saldo Actual");
                Console.WriteLine(separator);
                foreach (var m in movimientos.OrderByDescending(m => m.Numero))
                {
                    string cuenta = $"{m.CodigoCuenta} - {m.NombreCliente}";
                    Console.WriteLine(
                        $"{Truncate(cuenta, 28),-28} | " +
                        $"{m.Numero,-6} | " +
                        $"{m.Fecha,-10:dd/MM/yyyy} | " +
                        $"{Truncate(m.Tipo, 14),-14} | " +
                        $"{m.Importe,8:F2} | " +
                        $"{m.SaldoActual,10:F2}");
                }
                Console.WriteLine(separator);
            }
            else
            {
                Console.WriteLine("No hay movimientos.");
            }
        }

        public MovimientoRequest? GetCrearMovimientoData(List<string> tipos)
        {
            Console.WriteLine("\n--- Crear Nuevo Movimiento ---");
            Console.WriteLine("Tipos disponibles:");
            for (int i = 0; i < tipos.Count; i++)
                Console.WriteLine($"{i + 1}. {tipos[i]}");

            Console.Write("Seleccione tipo (número): ");
            if (!int.TryParse(Console.ReadLine(), out int index) || index < 1 || index > tipos.Count)
            {
                ShowError("Selección inválida.");
                return null;
            }
            string tipo = tipos[index - 1];

            Console.Write("Cuenta Origen: ");
            string? origen = Console.ReadLine();
            if (string.IsNullOrEmpty(origen)) return null;

            string? destino = null;
            if (tipo == "TRANSFERENCIA")
            {
                Console.Write("Cuenta Destino: ");
                destino = Console.ReadLine();
                if (string.IsNullOrEmpty(destino)) return null;
            }

            Console.Write("Monto: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal importe) || importe <= 0)
            {
                ShowError("Monto inválido.");
                return null;
            }

            return new MovimientoRequest { Tipo = tipo, CuentaOrigen = origen, CuentaDestino = destino, Importe = importe };
        }

        private static string Truncate(string? value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return "";
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
    }
}