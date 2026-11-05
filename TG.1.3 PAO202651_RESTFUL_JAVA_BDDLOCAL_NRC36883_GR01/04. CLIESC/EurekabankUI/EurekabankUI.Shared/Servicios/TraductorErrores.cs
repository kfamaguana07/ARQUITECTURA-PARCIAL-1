using System.Net.Http;
using System.Net.Sockets;
using ec.edu.monster.Modelos;

namespace ec.edu.monster.Servicios;

/// <summary>
/// Traduce excepciones técnicas (red, timeout, DNS) a mensajes claros para el usuario.
/// Se usa en Login, Conversión y Cambio de contraseña para mantener consistencia.
/// </summary>
public static class TraductorErrores
{
    public static string MensajeAmigable(Exception ex, TipoBackend backend)
    {
        string servicio = NombreServicio(backend);
        string destino = Destino(backend);

        if (EsTimeout(ex))
            return $"El servicio {servicio} tardó demasiado en responder. Revisa tu conexión e inténtalo de nuevo.";

        if (EsConexionRechazada(ex))
            return $"No se pudo conectar con el servicio {servicio} ({destino}). " +
                   "Verifica que el servidor esté en ejecución y que la dirección de red sea accesible.";

        if (EsHostNoEncontrado(ex))
            return $"No se encontró el servidor {servicio} ({destino}). " +
                   "Revisa la dirección configurada en ConstantesRed.";

        if (EsErrorDeRed(ex))
            return $"No se pudo establecer conexión con el servicio {servicio} ({destino}). " +
                   "Comprueba tu conexión de red e inténtalo nuevamente.";

        // Errores de negocio (credenciales, validaciones, HTTP, SOAP Fault) ya vienen con texto legible.
        return ex.Message;
    }

    private static string NombreServicio(TipoBackend tipo) => tipo switch
    {
        TipoBackend.JavaSoap => "Java SOAP",
        TipoBackend.JavaRest => "Java REST",
        TipoBackend.DotNetSoap => ".NET SOAP",
        TipoBackend.DotNetRest => ".NET REST",
        _ => "seleccionado"
    };

    private static string Endpoint(TipoBackend tipo) => tipo switch
    {
        TipoBackend.JavaSoap => ConstantesRed.UrlJavaSoap,
        TipoBackend.JavaRest => ConstantesRed.UrlJavaRest,
        TipoBackend.DotNetSoap => ConstantesRed.UrlDotNetSoapLogin,
        TipoBackend.DotNetRest => ConstantesRed.UrlDotNetRest,
        _ => ConstantesRed.IP_WEB_SERVICES
    };

    private static string Destino(TipoBackend tipo)
    {
        try
        {
            var uri = new Uri(Endpoint(tipo));
            return $"{uri.Host}:{uri.Port}";
        }
        catch
        {
            return ConstantesRed.IP_WEB_SERVICES;
        }
    }

    private static bool EsTimeout(Exception ex) =>
        CadenaDeExcepciones(ex).Any(e => e is TimeoutException or TaskCanceledException or OperationCanceledException);

    private static bool EsConexionRechazada(Exception ex)
    {
        var raiz = ObtenerRaiz(ex);
        if (raiz is SocketException se &&
            se.SocketErrorCode is SocketError.ConnectionRefused or SocketError.ConnectionReset)
            return true;

        return CadenaDeExcepciones(ex).Any(e =>
            e.Message.Contains("actively refused", StringComparison.OrdinalIgnoreCase) ||
            e.Message.Contains("connection refused", StringComparison.OrdinalIgnoreCase) ||
            e.Message.Contains("No connection could be made", StringComparison.OrdinalIgnoreCase));
    }

    private static bool EsHostNoEncontrado(Exception ex)
    {
        var raiz = ObtenerRaiz(ex);
        if (raiz is SocketException se && se.SocketErrorCode == SocketError.HostNotFound)
            return true;

        return CadenaDeExcepciones(ex).Any(e =>
            e.Message.Contains("No such host is known", StringComparison.OrdinalIgnoreCase) ||
            e.Message.Contains("host desconocido", StringComparison.OrdinalIgnoreCase));
    }

    private static bool EsErrorDeRed(Exception ex) =>
        CadenaDeExcepciones(ex).Any(e => e is HttpRequestException or SocketException);

    private static IEnumerable<Exception> CadenaDeExcepciones(Exception ex)
    {
        for (Exception? actual = ex; actual is not null; actual = actual.InnerException)
            yield return actual;
    }

    private static Exception ObtenerRaiz(Exception ex)
    {
        var actual = ex;
        while (actual.InnerException is not null)
            actual = actual.InnerException;
        return actual;
    }
}
