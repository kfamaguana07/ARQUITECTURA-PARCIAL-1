
namespace Comercializadora_ClienteConsola_Dotnet_GR01.Views
{
    public static class ConsoleHelper
    {
        public static void MostrarTitulo(string titulo)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("================================================================================");
            Console.WriteLine($"   {titulo.ToUpper()}");
            Console.WriteLine("================================================================================");
            Console.ResetColor();
            Console.WriteLine();
        }

        public static void MostrarMensajeExito(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n[ÉXITO] {mensaje}");
            Console.ResetColor();
        }

        public static void MostrarMensajeError(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n[ERROR] {mensaje}");
            Console.ResetColor();
        }

        public static void MostrarMensajeInfo(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n[INFO] {mensaje}");
            Console.ResetColor();
        }

        public static string LeerTextoRequerido(string etiqueta)
        {
            string entrada;
            do
            {
                Console.Write($"{etiqueta}: ");
                entrada = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(entrada))
                {
                    MostrarMensajeError("El campo no puede estar vacío. Intente de nuevo.");
                }
            } while (string.IsNullOrWhiteSpace(entrada));

            return entrada;
        }

        public static int LeerEntero(string etiqueta, int min = int.MinValue, int max = int.MaxValue)
        {
            int valor;
            while (true)
            {
                Console.Write($"{etiqueta}: ");
                if (int.TryParse(Console.ReadLine(), out valor) && valor >= min && valor <= max)
                {
                    return valor;
                }
                MostrarMensajeError($"Debe ingresar un número entero válido entre {min} y {max}.");
            }
        }

        public static decimal LeerDecimal(string etiqueta, decimal min = 0.01m)
        {
            decimal valor;
            while (true)
            {
                Console.Write($"{etiqueta}: ");
                if (decimal.TryParse(Console.ReadLine(), out valor) && valor >= min)
                {
                    return valor;
                }
                MostrarMensajeError($"Debe ingresar un valor numérico/decimal mayor o igual a {min}.");
            }
        }

        public static void Pausar()
        {
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
}

