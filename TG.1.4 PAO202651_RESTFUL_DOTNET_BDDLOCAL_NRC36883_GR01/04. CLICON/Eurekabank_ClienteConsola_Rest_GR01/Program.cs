using Eurekabank_ClienteConsola_Rest_GR01.Services;
using Eurekabank_ClienteConsola_Rest_GR01.Views;


namespace Eurekabank_ClienteConsola_Dotnet_GR01
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Configurar UTF-8 para evitar problemas con tildes y caracteres especiales
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            // Instanciamos el servicio y la vista
            var service = new EuBankApiService();
            var view = new ConsoleView();

            string? currentUser = null;
            view.ShowWelcome();

            // Bucle principal de la aplicación
            while (true)
            {
                if (currentUser == null)
                {
                    // === FLUJO DE LOGIN ===
                    var (usuario, clave) = view.GetLoginCredentials();

                    if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(clave))
                    {
                        view.ShowError("Usuario y contraseña son requeridos.");
                        continue;
                    }

                    // Llamada a la API REST
                    var resultado = await service.LoginAsync(usuario, clave);

                    if (resultado.Codigo == 1) // 1 = Éxito
                    {
                        currentUser = usuario;
                        view.ShowLoginSuccess(currentUser);
                    }
                    else
                    {
                        view.ShowError(resultado.Mensaje);
                    }
                }
                else
                {
                    // === FLUJO DEL DASHBOARD ===
                    int opcion = view.ShowDashboardMenu();

                    switch (opcion)
                    {
                        case 1: // Ver Cuentas
                            var cuentas = await service.GetCuentasAsync();
                            view.DisplayCuentas(cuentas);
                            break;

                        case 2: // Ver Movimientos
                            var filtroCuenta = view.GetMovimientoFilter();
                            var movimientos = await service.GetMovimientosAsync(filtroCuenta);
                            view.DisplayMovimientos(movimientos);
                            break;

                        case 3: // Crear Movimiento
                            var tipos = service.GetTiposMovimiento();
                            var request = view.GetCrearMovimientoData(tipos);

                            if (request != null)
                            {
                                var resp = await service.RegistrarMovimientoAsync(request);
                                if (resp.Codigo == 1)
                                {
                                    view.ShowLoginSuccess("¡Movimiento registrado exitosamente!");
                                }
                                else
                                {
                                    view.ShowError(resp.Mensaje);
                                }
                            }
                            break;

                        case 4: // Salir
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("\nSesión cerrada. ¡Gracias por usar EuBank!");
                            Console.ResetColor();
                            return;

                        default:
                            view.ShowError("Opción inválida. Por favor, seleccione una opción del 1 al 4.");
                            break;
                    }
                }
            }
        }
    }
}