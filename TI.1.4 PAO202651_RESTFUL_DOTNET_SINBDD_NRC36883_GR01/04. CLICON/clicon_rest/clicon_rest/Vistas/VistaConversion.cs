using ec.edu.monster.Controladores;

namespace ec.edu.monster.Cliente.Vistas;

public class VistaConversion
{
    private readonly string[] _unidadesLongitud = { "MILIMETRO", "CENTIMETRO", "METRO", "KILOMETRO", "YARDA" };
    private readonly string[] _unidadesMasa = { "MILIGRAMO", "GRAMO", "KILOGRAMO", "TONELADA", "ONZA" };
    private readonly string[] _unidadesTemperatura = { "CELSIUS", "FAHRENHEIT", "KELVIN", "RANKINE" };

    private readonly ControladorAplicacion _controlador;

    public VistaConversion(ControladorAplicacion controlador)
    {
        _controlador = controlador;
    }

    public async Task EjecutarMenuAsync()
    {
        int opcion;
        do
        {
            Console.WriteLine("\n=== MENÚ PRINCIPAL (.NET REST) ===");
            Console.WriteLine("1. Convertir Longitud");
            Console.WriteLine("2. Convertir Masa");
            Console.WriteLine("3. Convertir Temperatura");
            Console.WriteLine("4. Cambiar Contraseña");
            Console.WriteLine("0. Salir y Cerrar Sesión");
            Console.Write("Seleccione una opción: ");

            opcion = LeerEntero();

            switch (opcion)
            {
                case 1: await ProcesarConversionAsync("longitud", _unidadesLongitud); break;
                case 2: await ProcesarConversionAsync("masa", _unidadesMasa); break;
                case 3: await ProcesarConversionAsync("temperatura", _unidadesTemperatura); break;
                case 4: await CambiarContrasenaAsync(); break;
                case 0: Console.WriteLine("Saliendo del menú de conversión..."); break;
                default: Console.WriteLine("Opción no válida. Intente nuevamente."); break;
            }
        } while (opcion != 0);
    }

    private async Task ProcesarConversionAsync(string tipoMagnitud, string[] unidadesPermitidas)
    {
        Console.WriteLine($"\n--- CONVERSIÓN DE {tipoMagnitud.ToUpper()} ---");

        Console.Write("Ingrese el valor a convertir: ");
        double valor = LeerDecimal();

        string unidadInicial = SolicitarUnidad("inicial", unidadesPermitidas);
        string unidadFinal = SolicitarUnidad("final", unidadesPermitidas);

        try
        {
            double resultado = await _controlador.ConvertirAsync(tipoMagnitud, valor, unidadInicial, unidadFinal);
            Console.WriteLine($"Resultado: {valor:F4} {unidadInicial} = {resultado:F4} {unidadFinal}\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {ex.Message}");
        }
    }

    private string SolicitarUnidad(string tipoUnidad, string[] unidadesPermitidas)
    {
        Console.WriteLine($"Seleccione la unidad {tipoUnidad}:");
        for (int i = 0; i < unidadesPermitidas.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {unidadesPermitidas[i]}");
        }

        while (true)
        {
            Console.Write("Opción de unidad: ");
            int opcion = LeerEntero();
            if (opcion >= 1 && opcion <= unidadesPermitidas.Length)
            {
                return unidadesPermitidas[opcion - 1];
            }
            Console.WriteLine("Unidad no válida. Elija una opción de la lista.");
        }
    }

    private async Task CambiarContrasenaAsync()
    {
        Console.Write("\nContraseña actual: ");
        string actual = Console.ReadLine() ?? "";
        Console.Write("Nueva contraseña: ");
        string nueva;

        // Bucle infinito controlado para validar que las contraseñas coincidan
        while (true)
        {
            nueva = this.PedirDato("Nueva contraseña (o '-1' para cancelar)");

            if (nueva == "-1")
            {
                this.MostrarMensaje("Operación cancelada. Regresando al menú...");
                return; // Corta la ejecución y vuelve al menú principal
            }

            string confirmacion = this.PedirDato("Confirmar nueva contraseña");

            if (nueva == confirmacion)
            {
                break; // Rompe el bucle porque son iguales
            }
            else
            {
                this.MostrarError("Las contraseñas no coinciden. Intente de nuevo.\n");
            }
        }
        try
        {
            await _controlador.CambiarContrasenaAsync(actual, nueva);
            Console.WriteLine("Contraseña actualizada correctamente.");
            Console.WriteLine("Por seguridad, cierre la sesion y vuelva a ingresar.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {ex.Message}");
        }
    }

    private int LeerEntero()
    {
        int valor;
        while (!int.TryParse(Console.ReadLine(), out valor))
        {
            Console.Write("Por favor, ingrese un número entero válido: ");
        }
        return valor;
    }

    private double LeerDecimal()
    {
        double valor;
        while (!double.TryParse(Console.ReadLine(), out valor))
        {
            Console.Write("Por favor, ingrese un número válido (use coma/punto según su sistema): ");
        }
        return valor;
    }

    public string PedirDato(string prompt)
    {
        Console.Write($"{prompt}: ");
        return Console.ReadLine()?.Trim();
    }

    public void MostrarMensaje(string mensaje) => Console.WriteLine(mensaje);

    public void MostrarError(string mensaje)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[ERROR] {mensaje}");
        Console.ResetColor();
    }
}