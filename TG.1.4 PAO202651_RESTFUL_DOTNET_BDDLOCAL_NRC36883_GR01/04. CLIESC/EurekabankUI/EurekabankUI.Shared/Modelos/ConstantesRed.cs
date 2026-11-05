namespace ec.edu.monster.Modelos;

public static class ConstantesRed
{
    public const string IP_WEB_SERVICES = "10.96.24.79";
    // public const string IP_WEB_SERVICES = "10.206.225.79";

    public const string UrlJavaSoap = $"http://{IP_WEB_SERVICES}:8080/WS_eurekabank_Java_Soap_GR01/WSEurekaBank";
    public const string UrlDotNetSoapLogin = $"http://{IP_WEB_SERVICES}:2929/Services/LoginService.svc";
    public const string UrlDotNetSoapMovimientos = $"http://{IP_WEB_SERVICES}:2929/Services/MovimientosService.svc";
    public const string UrlDotNetRest = $"http://{IP_WEB_SERVICES}:3030/api";
    public const string UrlJavaRest = $"http://{IP_WEB_SERVICES}:8080/WS_Eurekabank_Java_Rest_GR01/api/eurekabank";
}