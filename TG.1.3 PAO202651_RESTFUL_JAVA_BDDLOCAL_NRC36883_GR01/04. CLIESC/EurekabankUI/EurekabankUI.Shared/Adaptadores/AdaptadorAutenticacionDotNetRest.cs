using System.Net.Http.Json;
using ec.edu.monster.Interfaces;
using ec.edu.monster.Modelos;

namespace ec.edu.monster.Adaptadores;

public class AdaptadorAutenticacionDotNetRest : IAdaptadorAutenticacion
{
    public TipoBackend Tipo => TipoBackend.DotNetRest;
    private readonly HttpClient _httpClient;

    public AdaptadorAutenticacionDotNetRest(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> IniciarSesionAsync(string usuario, string contrasenia)
    {
        // El LoginRequestDTO de .NET espera "usuario" y "contrasena"
        var payload = new { usuario = usuario, contrasena = contrasenia };

        try
        {
            var response = await _httpClient.PostAsJsonAsync($"{ConstantesRed.UrlDotNetRest}/login", payload);

            // Leemos la respuesta
            var resultado = await response.Content.ReadFromJsonAsync<ResultadoOperacion>();

            if (response.IsSuccessStatusCode && resultado != null && resultado.Codigo == 1)
            {
                return "OK";
            }

            throw new Exception(resultado?.Mensaje ?? "Credenciales incorrectas");
        }
        catch (HttpRequestException ex)
        {
            throw new Exception("Error de red al conectar con el servidor .NET REST: " + ex.Message);
        }
    }

}