using System.Globalization;
using System.Text;
using System.Xml.Linq;
using ec.edu.monster.Interfaces;
using ec.edu.monster.Modelos;

namespace ec.edu.monster.Adaptadores;

public class AdaptadorBancoDotNetSoap : IAdaptadorBanco
{
    public TipoBackend Tipo => TipoBackend.DotNetSoap;
    private readonly HttpClient _httpClient;

    public AdaptadorBancoDotNetSoap(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<DatosCuenta>> ObtenerCuentasAsync()
    {
        string soapXml = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"">
                               <soapenv:Header/>
                               <soapenv:Body>
                                  <tem:GetDatosCuentas/>
                               </soapenv:Body>
                            </soapenv:Envelope>";

        var response = await EnviarSoap(ConstantesRed.UrlDotNetSoapMovimientos, soapXml, "http://tempuri.org/IMovimientosService/GetDatosCuentas");
        var doc = XDocument.Parse(response);

        var cuentas = new List<DatosCuenta>();
        var elementos = doc.Descendants().Where(x => x.Name.LocalName == "CuentaData" || x.Name.LocalName == "DatosCuenta").ToList();

        foreach (var el in elementos)
        {
            cuentas.Add(new DatosCuenta
            {
                Codigo = ObtenerValorElemento(el, "Codigo"),
                NombreCliente = ObtenerValorElemento(el, "NombreCliente"),
                Saldo = decimal.Parse(ObtenerValorElemento(el, "Saldo", "0"), CultureInfo.InvariantCulture),
                Estado = ObtenerValorElemento(el, "Estado"),
                Moneda = ObtenerValorElemento(el, "Moneda"),
                EmailCliente = ObtenerValorElemento(el, "EmailCliente"),
                TelefonoCliente = ObtenerValorElemento(el, "TelefonoCliente")
            });
        }
        return cuentas;
    }

    public async Task<List<MovimientoData>> ObtenerMovimientosAsync(string cuenta)
    {
        string soapXml = $@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"">
                               <soapenv:Header/>
                               <soapenv:Body>
                                  <tem:GetMovimientos>
                                     <tem:cuenta>{cuenta}</tem:cuenta>
                                  </tem:GetMovimientos>
                               </soapenv:Body>
                            </soapenv:Envelope>";

        var response = await EnviarSoap(ConstantesRed.UrlDotNetSoapMovimientos, soapXml, "http://tempuri.org/IMovimientosService/GetMovimientos");
        var doc = XDocument.Parse(response);

        var movimientos = new List<MovimientoData>();
        var elementos = doc.Descendants().Where(x => x.Name.LocalName == "MovimientoData").ToList();

        foreach (var el in elementos)
        {
            movimientos.Add(new MovimientoData
            {
                CodigoCuenta = ObtenerValorElemento(el, "CodigoCuenta"),
                Numero = int.Parse(ObtenerValorElemento(el, "Numero", "0")),
                Fecha = DateTime.Parse(ObtenerValorElemento(el, "Fecha", DateTime.Now.ToString("s")), CultureInfo.InvariantCulture),
                Tipo = ObtenerValorElemento(el, "Tipo"),
                Importe = decimal.Parse(ObtenerValorElemento(el, "Importe", "0"), CultureInfo.InvariantCulture),
                Referencia = ObtenerValorElemento(el, "Referencia"),
                SaldoActual = decimal.Parse(ObtenerValorElemento(el, "SaldoActual", "0"), CultureInfo.InvariantCulture),
                NombreCliente = ObtenerValorElemento(el, "NombreCliente")
            });
        }
        return movimientos;
    }

    public async Task<ResultadoOperacion> RegistrarMovimientoAsync(string cuentaOrigen, string cuentaDestino, string tipo, double importe)
    {
        string destino = string.IsNullOrEmpty(cuentaDestino) ? "" : cuentaDestino;
        string importeStr = importe.ToString(CultureInfo.InvariantCulture);

        string soapXml = $@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"">
                           <soapenv:Header/>
                           <soapenv:Body>
                              <tem:ProcesarMovimiento>
                                 <tem:tipo>{tipo}</tem:tipo>
                                 <tem:cuentaOrigen>{cuentaOrigen}</tem:cuentaOrigen>
                                 <tem:cuentaDestino>{destino}</tem:cuentaDestino>
                                 <tem:importe>{importeStr}</tem:importe>
                              </tem:ProcesarMovimiento>
                           </soapenv:Body>
                        </soapenv:Envelope>";

        var response = await EnviarSoap(ConstantesRed.UrlDotNetSoapMovimientos, soapXml, "http://tempuri.org/IMovimientosService/ProcesarMovimiento");
        var doc = XDocument.Parse(response);

        // WCF retorna en la etiqueta <ProcesarMovimientoResult>
        var resultadoEl = doc.Descendants().FirstOrDefault(x => x.Name.LocalName == "ProcesarMovimientoResult" || x.Name.LocalName == "ResultadoOperacion");

        if (resultadoEl == null)
            return new ResultadoOperacion { Codigo = -1, Mensaje = "Error al procesar la respuesta WCF." };

        return new ResultadoOperacion
        {
            Codigo = int.Parse(ObtenerValorElemento(resultadoEl, "Codigo", "-1")),
            Mensaje = ObtenerValorElemento(resultadoEl, "Mensaje", "Error desconocido")
        };
    }

    private async Task<string> EnviarSoap(string url, string soapXml, string soapAction)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Content = new StringContent(soapXml, Encoding.UTF8, "text/xml");
        request.Headers.Add("SOAPAction", $"\"{soapAction}\"");

        var response = await _httpClient.SendAsync(request);
        var responseXml = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"--- ERROR WCF {soapAction} ---");
            Console.WriteLine(responseXml);

            try
            {
                var docFault = XDocument.Parse(responseXml);
                var faultString = docFault.Descendants().FirstOrDefault(x => x.Name.LocalName == "faultstring")?.Value;
                throw new Exception($"Error SOAP .NET: {faultString ?? response.StatusCode.ToString()}");
            }
            catch (System.Xml.XmlException)
            {
                throw new Exception($"Error HTTP: {response.StatusCode}");
            }
        }

        return responseXml;
    }

    // Método auxiliar para buscar elementos sin importar si cambian los Namespaces (http://schemas.datacontract.org/...)
    private static string ObtenerValorElemento(XElement padre, string nombreElemento, string valorPorDefecto = "")
    {
        var el = padre.Elements().FirstOrDefault(x => x.Name.LocalName == nombreElemento);
        return el?.Value ?? valorPorDefecto;
    }
}