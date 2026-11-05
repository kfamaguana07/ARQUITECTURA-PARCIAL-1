using System.Net.Http.Json;
using ec.edu.monster.Interfaces;
using ec.edu.monster.Modelos;

namespace ec.edu.monster.Adaptadores;

public class AdaptadorBancoDotNetRest : IAdaptadorBanco
{
    public TipoBackend Tipo => TipoBackend.DotNetRest;
    private readonly HttpClient _httpClient;

    public AdaptadorBancoDotNetRest(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<DatosCuenta>> ObtenerCuentasAsync()
    {
        try
        {
            // El endpoint GET es /api/movimientos/cuentas
            var cuentas = await _httpClient.GetFromJsonAsync<List<DatosCuenta>>($"{ConstantesRed.UrlDotNetRest}/movimientos/cuentas");
            return cuentas ?? new List<DatosCuenta>();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener cuentas desde .NET REST: " + ex.Message);
        }
    }

    public async Task<List<MovimientoData>> ObtenerMovimientosAsync(string cuenta)
    {
        try
        {
            // El endpoint GET es /api/movimientos?cuenta=XXXX
            var url = $"{ConstantesRed.UrlDotNetRest}/movimientos";
            if (!string.IsNullOrEmpty(cuenta))
            {
                url += $"?cuenta={cuenta}";
            }

            var movimientos = await _httpClient.GetFromJsonAsync<List<MovimientoData>>(url);
            return movimientos ?? new List<MovimientoData>();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener movimientos desde .NET REST: " + ex.Message);
        }
    }

    public async Task<ResultadoOperacion> RegistrarMovimientoAsync(string cuentaOrigen, string cuentaDestino, string tipo, double importe)
    {
        // Armamos el JSON que espera tu MovimientoRequestDTO
        var payload = new
        {
            cuentaOrigen = cuentaOrigen,
            cuentaDestino = cuentaDestino ?? "",
            tipo = tipo,
            importe = importe
        };

        try
        {
            // El endpoint POST es /api/movimientos
            var response = await _httpClient.PostAsJsonAsync($"{ConstantesRed.UrlDotNetRest}/movimientos", payload);

            var resultado = await response.Content.ReadFromJsonAsync<ResultadoOperacion>();
            
            if (resultado != null)
            {
                return resultado;
            }

            return new ResultadoOperacion { Codigo = -1, Mensaje = "No se recibió respuesta válida del servidor." };
        }
        catch (Exception ex)
        {
            return new ResultadoOperacion { Codigo = -1, Mensaje = "Error de conexión: " + ex.Message };
        }
    }
}