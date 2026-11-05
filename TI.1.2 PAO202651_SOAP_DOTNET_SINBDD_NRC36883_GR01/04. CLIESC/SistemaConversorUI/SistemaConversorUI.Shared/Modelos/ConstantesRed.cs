namespace ec.edu.monster.Modelos;

public static class ConstantesRed
{
    //public const string IP_WEB_SERVICES = "192.168.18.13";
    public const string IP_WEB_SERVICES = "172.29.176.1";

    public const string UrlJavaSoap = $"http://{IP_WEB_SERVICES}:8080/WS_ConUni_Java_SOAP_GR01/WSConversorUnidades";
    public const string UrlDotNetSoap = $"http://{IP_WEB_SERVICES}:8081/WSConversorUnidades.svc";
    public const string UrlDotNetRest = $"http://{IP_WEB_SERVICES}:5135/api";
    public const string UrlJavaRest = $"http://{IP_WEB_SERVICES}:8080/WS_ConUni_Java_Rest_GR01/api";
}
