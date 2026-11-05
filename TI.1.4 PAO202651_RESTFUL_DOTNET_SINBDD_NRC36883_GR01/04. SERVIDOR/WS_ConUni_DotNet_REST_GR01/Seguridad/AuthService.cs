using System.Collections.Concurrent;

namespace WS_ConUni_DotNet_Rest_GR5.Seguridad;

public class AuthService
{
    private const string UsuarioPermitido = "Monster";
    private readonly TokenProvider _tokenProvider;
    private readonly ConcurrentDictionary<string, DateTime> _tokensActivos = new();
    private readonly object _lock = new();
    private string _contrasenaActual = "Monster9";

    public AuthService(TokenProvider tokenProvider)
    {
        _tokenProvider = tokenProvider;
    }

    public string? Autenticar(string usuario, string contrasena)
    {
        if (!string.Equals(usuario, UsuarioPermitido, StringComparison.Ordinal) ||
            !string.Equals(contrasena, _contrasenaActual, StringComparison.Ordinal))
        {
            return null;
        }

        var token = _tokenProvider.GenerarToken(usuario);
        _tokensActivos[token] = DateTime.UtcNow.AddHours(8);
        return token;
    }

    public bool EsTokenValido(string token)
    {
        if (!_tokensActivos.TryGetValue(token, out var expiraEn))
        {
            return false;
        }

        if (DateTime.UtcNow <= expiraEn)
        {
            return true;
        }

        _tokensActivos.TryRemove(token, out _);
        return false;
    }

    public bool CambiarContrasena(string contrasenaActual, string nuevaContrasena)
    {
        if (string.IsNullOrWhiteSpace(nuevaContrasena))
        {
            return false;
        }

        lock (_lock)
        {
            if (!string.Equals(_contrasenaActual, contrasenaActual, StringComparison.Ordinal))
            {
                return false;
            }

            _contrasenaActual = nuevaContrasena;
            _tokensActivos.Clear();
            return true;
        }
    }

    public string UsuarioAutorizado => UsuarioPermitido;
}
