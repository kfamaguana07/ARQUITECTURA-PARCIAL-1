using Eurekabank_ClienteConsola_Rest_GR01.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace Eurekabank_ClienteConsola_Rest_GR01.Services
{
    public class EuBankApiService
    {
        private readonly HttpClient _httpClient;
        // Ajusta el puerto
        private const string BaseUrl = "http://localhost:3030/api";

        public EuBankApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        public async Task<ResultadoOperacion> LoginAsync(string usuario, string contrasena)
        {
            try
            {
                var request = new LoginRequest { Usuario = usuario, Contrasena = contrasena };
                var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/login", request);

                var resultado = await response.Content.ReadFromJsonAsync<ResultadoOperacion>();
                return resultado ?? new ResultadoOperacion { Codigo = -1, Mensaje = "Error desconocido" };
            }
            catch (Exception ex)
            {
                return new ResultadoOperacion { Codigo = -1, Mensaje = "Error de conexión: " + ex.Message };
            }
        }

        public async Task<List<Cuenta>> GetCuentasAsync()
        {
            try
            {
                var cuentas = await _httpClient.GetFromJsonAsync<List<Cuenta>>($"{BaseUrl}/movimientos/cuentas");
                return cuentas ?? new List<Cuenta>();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener cuentas: " + ex.Message);
                return new List<Cuenta>();
            }
        }

        public async Task<List<Movement>> GetMovimientosAsync(string cuenta)
        {
            try
            {
                string url = $"{BaseUrl}/movimientos";
                if (!string.IsNullOrEmpty(cuenta))
                    url += $"?cuenta={cuenta}";

                var movimientos = await _httpClient.GetFromJsonAsync<List<Movement>>(url);
                return movimientos ?? new List<Movement>();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener movimientos: " + ex.Message);
                return new List<Movement>();
            }
        }

        public async Task<ResultadoOperacion> RegistrarMovimientoAsync(MovimientoRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/movimientos", request);
                var resultado = await response.Content.ReadFromJsonAsync<ResultadoOperacion>();
                return resultado ?? new ResultadoOperacion { Codigo = -1, Mensaje = "Error desconocido" };
            }
            catch (Exception ex)
            {
                return new ResultadoOperacion { Codigo = -1, Mensaje = "Error de conexión: " + ex.Message };
            }
        }

        public List<string> GetTiposMovimiento()
        {
            // Como en REST no hay un endpoint para tipos (o si lo hay, agrégalo)
            // Podemos hardcodearlos o consumir el endpoint si lo implementas
            return new List<string> { "DEPOSITO", "RETIRO", "TRANSFERENCIA" };
        }
    }
}