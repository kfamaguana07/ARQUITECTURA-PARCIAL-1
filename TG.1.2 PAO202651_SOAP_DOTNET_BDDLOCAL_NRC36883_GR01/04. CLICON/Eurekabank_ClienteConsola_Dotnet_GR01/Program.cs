using Eurekabank_ClienteConsola_Dotnet_GR01.Models;
using Eurekabank_ClienteConsola_Dotnet_GR01.Services;
using Eurekabank_ClienteConsola_Dotnet_GR01.Views;

partial class Program
{
    static async Task Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.InputEncoding = System.Text.Encoding.UTF8;

        var service = new EuBankApiService();
        var view = new ConsoleView();

        string? currentUser = null;

        view.ShowWelcome();

        while (true)
        {
            if (currentUser == null)
            {
                // Login
                var (usuario, clave) = view.GetLoginCredentials();

                if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(clave))
                {
                    view.ShowMessage("Usuario y clave son requeridos.");
                    continue;
                }

                try
                {
                    var loginRequest = new LoginRequest { Usuario = usuario, Clave = clave };
                    if (await service.LoginAsync(loginRequest))
                    {
                        currentUser = usuario;
                        view.ShowLoginSuccess(currentUser);
                    }
                    else
                    {
                        view.ShowLoginError("Error en login");
                    }
                }
                catch (Exception ex)
                {
                    view.ShowError(ex.Message);
                }
            }
            else
            {
                // Dashboard
                int opcion = view.ShowDashboardMenu();

                switch (opcion)
                {
                    case 1:
                        try
                        {
                            var cuentas = await service.GetAllAccountsAsync();
                            view.DisplayCuentas(cuentas ?? new List<Account>());
                        }
                        catch (Exception ex)
                        {
                            view.ShowError(ex.Message);
                        }
                        break;
                    case 2:
                        try
                        {
                            var cuenta = view.GetMovimientoFilter();
                            var movimientos = await service.GetMovementsAsync(cuenta);
                            view.DisplayMovimientos(movimientos ?? new List<Movement>());
                        }
                        catch (Exception ex)
                        {
                            view.ShowError(ex.Message);
                        }
                        break;
                    case 3:
                        try
                        {
                            var tipos = await service.GetMovementTypesAsync();
                            if (tipos != null && tipos.Any())
                            {
                                var request = view.GetCrearMovimientoData(tipos);
                                if (request != null)
                                {
                                    var response = await service.ProcessMovementAsync(request);
                                    view.ShowMessage(response.Success ? "Movimiento creado" : $"Error: {response.Message}");
                                }
                            }
                            else
                            {
                                view.ShowMessage("No hay tipos de movimiento disponibles");
                            }
                        }
                        catch (Exception ex)
                        {
                            view.ShowError(ex.Message);
                        }
                        break;
                    case 4:
                        view.ShowMessage("Sesión cerrada");
                        currentUser = null;
                        break;
                    default:
                        view.ShowMessage("Opción inválida");
                        break;
                }
            }
        }
    }
}
