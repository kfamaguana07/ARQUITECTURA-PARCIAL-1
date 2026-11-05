using System.Text;
using System.Xml.Linq;
using ec.edu.monster.Interfaces;
using ec.edu.monster.Modelos;

namespace ec.edu.monster.Adaptadores;

public class AdaptadorAutenticacionJavaSoap : IAdaptadorAutenticacion
{
    public TipoBackend Tipo => TipoBackend.JavaSoap;
    private readonly HttpClient _httpClient;

    public AdaptadorAutenticacionJavaSoap(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }


    public async Task<string> IniciarSesionAsync(string usuario, string contrasenia)
    {
        // 1. El XML de envío (ya estaba bien)
        string soapXml = $@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ws=""http://ws.monster.edu.ec/"">
                           <soapenv:Header/>
                           <soapenv:Body>
                              <ws:iniciarSesion>
                                 <usuario>{usuario}</usuario>
                                 <contrasena>{contrasenia}</contrasena>
                              </ws:iniciarSesion>
                           </soapenv:Body>
                        </soapenv:Envelope>";

        var content = new StringContent(soapXml, Encoding.UTF8, "text/xml");

        // 2. Enviamos la petición
        var response = await _httpClient.PostAsync(ConstantesRed.UrlJavaSoap, content);
        string responseXml = await response.Content.ReadAsStringAsync();

        // 3. Validamos si el servidor devolvió un error HTTP (500)
        if (!response.IsSuccessStatusCode)
        {
            // Intentamos extraer el mensaje de error del SOAP Fault
            try
            {
                var docFault = XDocument.Parse(responseXml);
                var faultString = docFault.Descendants().FirstOrDefault(x => x.Name.LocalName == "faultstring")?.Value;
                throw new Exception($"Error del servidor: {faultString ?? response.StatusCode.ToString()}");
            }
            catch (System.Xml.XmlException)
            {
                throw new Exception($"Error HTTP: {response.StatusCode}");
            }
        }

        // 4. Parseamos la respuesta exitosa
        var doc = XDocument.Parse(responseXml);

        // Esta es la búsqueda infalible: buscamos cualquier elemento que se llame "return"
        var resultElement = doc.Descendants().FirstOrDefault(x => x.Name.LocalName == "return");

        if (resultElement == null)
        {
            // Si no encuentra "return", imprimimos el XML para diagnosticar
            throw new Exception($"No se recibió respuesta válida del servidor SOAP. XML recibido: {responseXml}");
        }

        // 5. Evaluamos el resultado
        if (resultElement.Value.Equals("true", StringComparison.OrdinalIgnoreCase))
        {
            return "OK";
        }

        throw new Exception("Credenciales incorrectas");
    }

}