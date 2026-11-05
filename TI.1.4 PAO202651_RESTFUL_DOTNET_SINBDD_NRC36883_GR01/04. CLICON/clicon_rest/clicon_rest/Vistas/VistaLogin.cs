using ec.edu.monster.Controladores;

namespace ec.edu.monster.Cliente.Vistas;

public class VistaLogin
{
    private readonly ControladorAplicacion _controlador;

    public VistaLogin(ControladorAplicacion controlador)
    {
        _controlador = controlador;
    }

    public async Task IniciarAsync()
    {
        MensajeBienvenida();
        Console.WriteLine("=== SISTEMA DE CONVERSIÓN DE UNIDADES - .NET REST - GR01 ===");

        while (true)
        {
            Console.Write("\nUsuario: ");
            string usuario = Console.ReadLine() ?? "";

            Console.Write("Contraseña: ");
            string contrasena = Console.ReadLine() ?? "";

            try
            {
                await _controlador.AutenticarAsync(usuario, contrasena);
                Console.WriteLine("Inicio de sesión exitoso. Token guardado.");

                var vistaConversion = new VistaConversion(_controlador);
                await vistaConversion.EjecutarMenuAsync();

                _controlador.CerrarSesion();
                Console.WriteLine("Sesión finalizada de forma segura.");
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }
        }
    }

    private void MensajeBienvenida()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(@"
         __  __                   _                  
        |  \\/  | ___  _ __   ___ | |_ ___  _ __      
        | |\\/| |/ _ \\| '_ \\ / __|| __/ _ \\| '__|     
        | |  | | (_) | | | |\\__ \\| ||  __/| |        
        |_|  |_|\\___/|_| |_||___/ \\__\\___||_|        ");
        Console.ResetColor();
    }
}