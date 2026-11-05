using System.Security.Cryptography;
using System.Text;

namespace WS_ConUni_DotNet_Rest_GR5.Seguridad;

public class TokenProvider
{
    public string GenerarToken(string usuario)
    {
        var payload = $"{usuario}:{DateTime.UtcNow.Ticks}:{RandomNumberGenerator.GetInt32(1000, 9999)}";
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(payload));
    }
}
