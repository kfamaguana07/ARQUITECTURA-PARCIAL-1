using Eurekabank_ClienteConsola_Dotnet_GR01.Models;
using System.Linq;

namespace Eurekabank_ClienteConsola_Dotnet_GR01.Views;

public class ConsoleView
{
    public void ShowWelcome()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("==============================");
        Console.WriteLine("Cliente Consola EurekaBank - GR01");
        Console.WriteLine("DOTNET - SOAP");
        Console.WriteLine("========================");
        Console.ResetColor();
    }

    public (string? usuario, string? clave) GetLoginCredentials()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("------------------------------");
        Console.Write("Usuario: ");
        Console.ResetColor();
        string? usuario = Console.ReadLine();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("Clave: ");
        Console.ResetColor();
        string? clave = Console.ReadLine();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("------------------------------");
        Console.ResetColor();
        return (usuario, clave);
    }

    public void ShowLoginSuccess(string user)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Bienvenido, {user}!");
        Console.WriteLine("========================");
        Console.ResetColor();
    }

    public void ShowLoginError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Error: {message}");
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
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("=== Dashboard ===");
        Console.WriteLine("==================");
        Console.ResetColor();
        Console.WriteLine("1. Ver Cuentas");
        Console.WriteLine("2. Ver Movimientos");
        Console.WriteLine("3. Crear Movimiento");
        Console.WriteLine("4. Salir");
        Console.WriteLine("==================");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("Opcion: ");
        Console.ResetColor();
        string? input = Console.ReadLine();
        return int.TryParse(input, out int opcion) ? opcion : 0;
    }

    public void DisplayCuentas(List<Account> cuentas)
    {
        Console.WriteLine();
        if (cuentas.Any())
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Lista de Cuentas:");
            Console.WriteLine("=================");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Yellow;
            string separator = new string('-', 112);
            Console.WriteLine(separator);
            Console.WriteLine("Codigo   | Cliente                      | Email                   | Telefono    | Moneda | Saldo    | Estado");
            Console.WriteLine(separator);
            Console.ResetColor();
            foreach (var c in cuentas)
            {
                Console.WriteLine(
                    $"{c.Codigo,-7} | " +
                    $"{Truncate(c.NombreCliente, 28),-28} | " +
                    $"{Truncate(c.EmailCliente, 23),-23} | " + 
                    $"{Truncate(c.TelefonoCliente, 11),-11} | " + 
                    $"{c.Moneda,-6} | " +
                    $"{c.Saldo,8:F2} | " +
                    $"{c.Estado,-7}");
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(separator);
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No hay cuentas disponibles");
            Console.ResetColor();
        }
        Console.WriteLine();
    }
    private static string Truncate(string? value, int maxLength)
    {
        if (string.IsNullOrEmpty(value))
        {
            return "";
        }

        return value.Length <= maxLength ? value : value.Substring(0, maxLength);
    }

    public string? GetMovimientoFilter()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("Filtrar por cuenta (dejar vacio para todas): ");
        Console.ResetColor();
        return Console.ReadLine();
    }

    public void DisplayMovimientos(List<Movement> movimientos)
    {
        Console.WriteLine();
        if (movimientos.Any())
        {
            // Sort by Numero descending
            movimientos = movimientos.OrderByDescending(m => m.Numero).ToList();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Lista de Movimientos:");
            Console.WriteLine("=====================");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Yellow;
            string separator = new string('-', 112);
            Console.WriteLine(separator);
            Console.WriteLine("Cuenta                       | Numero | Fecha      | Tipo           | Referencia | Importe  | Saldo Actual");
            Console.WriteLine(separator);
            Console.ResetColor();
            foreach (var m in movimientos)
            {
                string cuenta = $"{m.CodigoCuenta} - {m.NombreCliente}";

                Console.WriteLine(
                    $"{Truncate(cuenta, 28),-28} | " +
                    $"{m.Numero,-6} | " +
                    $"{m.Fecha,-10:dd/MM/yyyy} | " +
                    $"{Truncate(m.Tipo, 14),-14} | " +
                    $"{Truncate(m.Referencia ?? "-", 10),-10} | " +
                    $"{m.Importe,8:F2} | " +
                    $"{m.SaldoActual,10:F2}");
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(separator);
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No hay movimientos");
            Console.ResetColor();
        }
        Console.WriteLine();
    }

    public MovementRequest? GetCrearMovimientoData(List<string> tipos)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Crear Nuevo Movimiento");
        Console.WriteLine("======================");
        Console.ResetColor();
        Console.WriteLine("Tipos disponibles:");
        for (int i = 0; i < tipos.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {tipos[i]}");
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("Seleccione tipo (numero): ");
        Console.ResetColor();
        if (!int.TryParse(Console.ReadLine(), out int tipoIndex) || tipoIndex < 1 || tipoIndex > tipos.Count)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Seleccion invalida");
            Console.ResetColor();
            return null;
        }

        string tipo = tipos[tipoIndex - 1];

        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("Cuenta Origen: ");
        Console.ResetColor();
        string? origen = Console.ReadLine();
        if (string.IsNullOrEmpty(origen))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Cuenta origen requerida");
            Console.ResetColor();
            return null;
        }

        string? destino = null;
        if (tipo == "TRANSFERENCIA")
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Cuenta Destino: ");
            Console.ResetColor();
            destino = Console.ReadLine();
            if (string.IsNullOrEmpty(destino))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Cuenta destino requerida para transferencias");
                Console.ResetColor();
                return null;
            }
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("Monto: ");
        Console.ResetColor();
        if (!decimal.TryParse(Console.ReadLine(), out decimal importe) || importe <= 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Monto invalido");
            Console.ResetColor();
            return null;
        }

        return new MovementRequest { Tipo = tipo, CuentaOrigen = origen, CuentaDestino = destino, Importe = importe };
    }

    public void ShowMessage(string message)
    {
        Console.WriteLine(message);
    }
}