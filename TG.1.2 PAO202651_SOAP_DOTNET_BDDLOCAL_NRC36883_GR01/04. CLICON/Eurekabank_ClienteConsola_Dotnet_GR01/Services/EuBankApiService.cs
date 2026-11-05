using ServiceReference1;
using ServiceReference2;
using Eurekabank_ClienteConsola_Dotnet_GR01.Models;

namespace Eurekabank_ClienteConsola_Dotnet_GR01.Services
{
    public class EuBankApiService
    {
        private readonly LoginServiceClient _loginClient;
        private readonly MovimientosServiceClient _movimientosClient;

        public EuBankApiService()
        {
            _loginClient = new LoginServiceClient();
            _movimientosClient = new MovimientosServiceClient();
        }

        public async Task<bool> LoginAsync(LoginRequest request)
        {
            try
            {
                return await _loginClient.LoginAsync(request.Usuario, request.Clave);
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Account>> GetAllAccountsAsync()
        {
            try
            {
                var cuentas = await _movimientosClient.GetDatosCuentasAsync();
                return cuentas.Select(c => new Account
                {
                    Codigo = c.Codigo,
                    NombreCliente = c.NombreCliente,
                    EmailCliente = c.EmailCliente,
                    TelefonoCliente = c.TelefonoCliente,
                    Moneda = c.Moneda,
                    Saldo = c.Saldo,
                    Estado = c.Estado
                }).ToList();
            }
            catch
            {
                return new List<Account>();
            }
        }

        public async Task<List<Movement>> GetMovementsAsync(string? cuenta)
        {
            try
            {
                var movimientos = await _movimientosClient.GetMovimientosAsync(cuenta ?? "");
                return movimientos.Select(m => new Movement
                {
                    Numero = m.Numero,
                    Fecha = m.Fecha,
                    Tipo = m.Tipo,
                    Importe = m.Importe,
                    Referencia = m.Referencia,
                    CodigoCuenta = m.CodigoCuenta,
                    SaldoActual = m.SaldoActual,
                    NombreCliente = m.NombreCliente
                }).ToList();
            }
            catch
            {
                return new List<Movement>();
            }
        }

        public async Task<List<string>> GetMovementTypesAsync()
        {
            try
            {
                var tipos = await _movimientosClient.GetTiposMovimientoAsync();
                return tipos.ToList();
            }
            catch
            {
                return new List<string>();
            }
        }

        public async Task<CrearMovimientoResponse> ProcessMovementAsync(MovementRequest request)
        {
            try
            {
                var result = await _movimientosClient.ProcesarMovimientoAsync(
                    request.Tipo,
                    request.CuentaOrigen,
                    request.CuentaDestino ?? "",
                    request.Importe);
                return new CrearMovimientoResponse
                {
                    Success = result.Codigo == 1,
                    Message = result.Mensaje
                };
            }
            catch (Exception ex)
            {
                return new CrearMovimientoResponse
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}