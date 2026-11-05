namespace ec.edu.monster.Modelos;

public static class ConstantesRed
{
    // En movil/emulador "localhost" es el propio telefono, no el PC.
    // Usar la IP LAN del PC que ejecuta Tomcat (ipconfig en PowerShell/CMD).
    //public const string IP_WEB_SERVICES = "localhost";
    public const string IP_WEB_SERVICES = "10.206.225.79";

    public const string UrlJavaSoap = $"http://{IP_WEB_SERVICES}:8080/WS_ConUni_Java_SOAP_GR01/WSConversorUnidades";
    public const string UrlDotNetSoap = $"http://{IP_WEB_SERVICES}:9099/WSConversorUnidades.svc";
    public const string UrlDotNetRest = $"http://{IP_WEB_SERVICES}:5170/api";
    public const string UrlJavaRest = $"http://{IP_WEB_SERVICES}:8080/WS_ConUni_Java_Rest_GR01/api";
}
