using System.Net.Http.Json;
using ec.edu.monster.Interfaces;
using ec.edu.monster.Modelos;

namespace ec.edu.monster.Adaptadores;

public class AdaptadorBancoJavaRest : IAdaptadorBanco
{
    public TipoBackend Tipo => TipoBackend.JavaRest;
    private readonly HttpClient _httpClient;

    public AdaptadorBancoJavaRest(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<DatosCuenta>> ObtenerCuentasAsync()
    {
        try
        {
            // Petición GET a /api/eurekabank/cuentas
            var cuentas = await _httpClient.GetFromJsonAsync<List<DatosCuenta>>($"{ConstantesRed.UrlJavaRest}/cuentas");
            return cuentas ?? new List<DatosCuenta>();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener cuentas desde REST: " + ex.Message);
        }
    }

    public async Task<List<MovimientoData>> ObtenerMovimientosAsync(string cuenta)
    {
        try
        {
            var url = $"{ConstantesRed.UrlJavaRest}/movimientos";
            if (!string.IsNullOrEmpty(cuenta))
            {
                url += $"?cuenta={cuenta}";
            }

            // 1. Recibimos los datos en un DTO temporal donde la fecha es String
            var movimientosDto = await _httpClient.GetFromJsonAsync<List<MovimientoDtoRest>>(url);

            if (movimientosDto == null) return new List<MovimientoData>();

            // 2. Mapeamos al modelo real (DateTime)
            var listaFinal = new List<MovimientoData>();

            foreach (var m in movimientosDto)
            {
                DateTime fechaParseada = DateTime.Now;

                // Intentamos parsear la fecha. Si tu Java envía "yyyy-MM-dd", esto funciona.
                if (!string.IsNullOrEmpty(m.fecha))
                {
                    DateTime.TryParse(m.fecha, out fechaParseada);
                }

                listaFinal.Add(new MovimientoData
                {
                    CodigoCuenta = m.codigoCuenta,
                    Numero = m.numero,
                    Fecha = fechaParseada,
                    Tipo = m.tipo,
                    Importe = m.importe,
                    Referencia = m.referencia,
                    SaldoActual = m.saldoActual,
                    NombreCliente = m.nombreCliente
                });
            }

            return listaFinal;
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener movimientos desde REST: " + ex.Message);
        }
    }

    // Clase interna para mapear el JSON de Java (donde 'fecha' es un String)
    private class MovimientoDtoRest
    {
        public string codigoCuenta { get; set; }
        public int numero { get; set; }
        public string fecha { get; set; } // <-- Aquí recibimos el String
        public string tipo { get; set; }
        public decimal importe { get; set; }
        public string referencia { get; set; }
        public decimal saldoActual { get; set; }
        public string nombreCliente { get; set; }
    }

    public async Task<ResultadoOperacion> RegistrarMovimientoAsync(string cuentaOrigen, string cuentaDestino, string tipo, double importe)
    {
        // Armamos el JSON que espera tu MovimientoRequestDto de Java
        var payload = new
        {
            cuentaOrigen = cuentaOrigen,
            cuentaDestino = cuentaDestino ?? "",
            tipo = tipo,
            importe = importe
        };

        try
        {
            // Petición POST a /api/eurekabank/movimientos
            var response = await _httpClient.PostAsJsonAsync($"{ConstantesRed.UrlJavaRest}/movimientos", payload);

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