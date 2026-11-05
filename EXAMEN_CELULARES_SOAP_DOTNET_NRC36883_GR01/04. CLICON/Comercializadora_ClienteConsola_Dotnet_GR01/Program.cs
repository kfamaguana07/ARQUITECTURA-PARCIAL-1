using Comercializadora_ClienteConsola_Dotnet_GR01.Controllers;
using System.Text;

namespace Cliente_Console_Comercializadora_GR01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Configurar consola para soporte de caracteres especiales y símbolos de moneda
            Console.OutputEncoding = Encoding.UTF8;
            Console.Title = "Sistema Comercializadora de Celulares - Cliente Consola MVC";

            // Arrancar el controlador principal
            MenuController menuController = new MenuController();
            menuController.Iniciar();
        }
    }
}