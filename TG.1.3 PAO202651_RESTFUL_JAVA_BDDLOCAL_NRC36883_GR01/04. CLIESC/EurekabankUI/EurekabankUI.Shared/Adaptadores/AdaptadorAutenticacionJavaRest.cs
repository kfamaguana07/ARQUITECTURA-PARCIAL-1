using System.Net.Http.Json;
using ec.edu.monster.Interfaces;
using ec.edu.monster.Modelos;

namespace ec.edu.monster.Adaptadores;

public class AdaptadorAutenticacionJavaRest : IAdaptadorAutenticacion
{
    public TipoBackend Tipo => TipoBackend.JavaRest;
    private readonly HttpClient _httpClient;

    public AdaptadorAutenticacionJavaRest(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> IniciarSesionAsync(string usuario, string contrasenia)
    {
        // Tu API REST espera un JSON con las propiedades "usuario" y "contrasena"
        var payload = new { usuario = usuario, contrasena = contrasenia };

        try
        {
            var response = await _httpClient.PostAsJsonAsync($"{ConstantesRed.UrlJavaRest}/login", payload);

            // Leemos la respuesta del servidor (ya sea 200 OK, 400 Bad Request o 500)
            var resultado = await response.Content.ReadFromJsonAsync<ResultadoOperacion>();

            if (response.IsSuccessStatusCode && resultado != null && resultado.Codigo == 1)
            {
                return "OK";
            }

            // Si no es exitoso, lanzamos el mensaje que devolvió el servidor
            throw new Exception(resultado?.Mensaje ?? "Credenciales incorrectas");
        }
        catch (HttpRequestException ex)
        {
            throw new Exception("Error de red al conectar con el servidor REST: " + ex.Message);
        }
    }

}