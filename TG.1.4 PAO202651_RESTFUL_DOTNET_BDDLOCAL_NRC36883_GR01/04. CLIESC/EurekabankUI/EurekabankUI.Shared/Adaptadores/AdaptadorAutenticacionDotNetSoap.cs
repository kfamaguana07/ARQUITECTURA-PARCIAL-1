using System.Text;
using System.Xml.Linq;
using ec.edu.monster.Interfaces;
using ec.edu.monster.Modelos;

namespace ec.edu.monster.Adaptadores;

public class AdaptadorAutenticacionDotNetSoap : IAdaptadorAutenticacion
{
    public TipoBackend Tipo => TipoBackend.DotNetSoap;
    private readonly HttpClient _httpClient;

    public AdaptadorAutenticacionDotNetSoap(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> IniciarSesionAsync(string usuario, string contrasenia)
    {
        string soapXml = $@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"">
                               <soapenv:Header/>
                               <soapenv:Body>
                                  <tem:Login>
                                     <tem:usuario>{usuario}</tem:usuario>
                                     <tem:clave>{contrasenia}</tem:clave>
                                  </tem:Login>
                               </soapenv:Body>
                            </soapenv:Envelope>";

        var request = new HttpRequestMessage(HttpMethod.Post, ConstantesRed.UrlDotNetSoapLogin);
        request.Content = new StringContent(soapXml, Encoding.UTF8, "text/xml");

        // ESTA CABECERA ES OBLIGATORIA EN WCF
        request.Headers.Add("SOAPAction", "\"http://tempuri.org/ILoginService/Login\"");

        var response = await _httpClient.SendAsync(request);
        string responseXml = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            // Si falla, imprimimos el XML para ver el error real
            Console.WriteLine("--- ERROR WCF LOGIN ---");
            Console.WriteLine(responseXml);
            throw new Exception($"Error HTTP WCF: {response.StatusCode}");
        }

        var doc = XDocument.Parse(responseXml);

        // Según tu WSDL, el resultado viene en <LoginResult>
        var resultElement = doc.Descendants().FirstOrDefault(x => x.Name.LocalName == "LoginResult");

        if (resultElement == null)
            throw new Exception("No se recibió respuesta válida del servidor WCF.");

        if (resultElement.Value.Equals("true", StringComparison.OrdinalIgnoreCase))
        {
            return "OK";
        }

        throw new Exception("Credenciales incorrectas");
    }

}