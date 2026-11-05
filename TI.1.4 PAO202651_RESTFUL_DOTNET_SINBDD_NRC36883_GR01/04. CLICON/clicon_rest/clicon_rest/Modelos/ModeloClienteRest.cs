using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ec.edu.monster.Modelos;

public class ModeloClienteRest
{
    private const string UrlBase = "http://localhost:5170/api";
    private readonly HttpClient _clienteHttp;
    private string? _tokenSesion;

    public ModeloClienteRest()
    {
        _clienteHttp = new HttpClient();
    }

    public bool EstaAutenticado => !string.IsNullOrEmpty(_tokenSesion);

    public void CerrarSesion()
    {
        _tokenSesion = null;
    }

    public async Task IniciarSesionAsync(string usuario, string contrasena)
    {
        var payload = new { usuario = usuario, contrasena = contrasena };

        var respuesta = await _clienteHttp.PostAsJsonAsync($"{UrlBase}/seguridad/login", payload);

        if (!respuesta.IsSuccessStatusCode)
        {
            throw new Exception("Credenciales incorrectas o error de servidor.");
        }

        var resultado = await respuesta.Content.ReadFromJsonAsync<LoginResponseDto>();
        _tokenSesion = resultado?.Token ?? throw new Exception("Token no recibido.");
    }

    public async Task CambiarContrasenaAsync(string contrasenaActual, string nuevaContrasena)
    {
        var payload = new { contrasenaActual = contrasenaActual, nuevaContrasena = nuevaContrasena };

        var peticion = new HttpRequestMessage(HttpMethod.Post, $"{UrlBase}/seguridad/cambiar-contrasena");
        peticion.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenSesion);
        peticion.Content = JsonContent.Create(payload);

        var respuesta = await _clienteHttp.SendAsync(peticion);

        if (!respuesta.IsSuccessStatusCode)
        {
            var error = await respuesta.Content.ReadAsStringAsync();
            throw new Exception($"No se pudo cambiar la contraseña: {error}");
        }
    }

    public async Task<double> ConvertirMagnitudAsync(string tipoMagnitud, double valor, string unidadOrigen, string unidadDestino)
    {
        var payload = new { valor = valor, unidadOrigen = unidadOrigen, unidadDestino = unidadDestino };

        var peticion = new HttpRequestMessage(HttpMethod.Post, $"{UrlBase}/conversiones/{tipoMagnitud}");
        peticion.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenSesion);
        peticion.Content = JsonContent.Create(payload);

        var respuesta = await _clienteHttp.SendAsync(peticion);

        if (!respuesta.IsSuccessStatusCode)
        {
            var error = await respuesta.Content.ReadAsStringAsync();
            throw new Exception($"Error en la conversión: {error}");
        }

        var resultado = await respuesta.Content.ReadFromJsonAsync<ConversionResponseDto>();
        return resultado?.ValorConvertido ?? 0;
    }

    private class LoginResponseDto { public string? Token { get; set; } }
    private class ConversionResponseDto { public double ValorConvertido { get; set; } }
}