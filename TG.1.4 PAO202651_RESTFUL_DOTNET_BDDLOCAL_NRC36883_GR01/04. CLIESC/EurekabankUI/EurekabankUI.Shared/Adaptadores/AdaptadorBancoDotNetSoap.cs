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
        // Nombre exacto del método en tu WSDL
        string soapXml = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"">
                               <soapenv:Header/>
                               <soapenv:Body>
                                  <tem:GetDatosCuentas/>
                               </soapenv:Body>
                            </soapenv:Envelope>";

        var response = await EnviarSoap(ConstantesRed.UrlDotNetSoapMovimientos, soapXml, "http://tempuri.org/IMovimientosService/GetDatosCuentas");

        // IMPRIMIMOS EN CONSOLA PARA VER QUÉ LLEGA
        Console.WriteLine("--- RESPUESTA WCF CUENTAS ---");
        Console.WriteLine(response);

        var doc = XDocument.Parse(response);
        var cuentas = new List<DatosCuenta>();

        // En WCF, si el método devuelve List<CuentaData>, el XML tendrá etiquetas <CuentaData>
        var elementos = doc.Descendants().Where(x => x.Name.LocalName == "CuentaData" || x.Name.LocalName == "DatosCuenta").ToList();

        foreach (var el in elementos)
        {
            cuentas.Add(new DatosCuenta
            {
                Codigo = el.Element(el.Name.Namespace + "Codigo")?.Value ?? "",
                NombreCliente = el.Element(el.Name.Namespace + "NombreCliente")?.Value ?? "",
                Saldo = decimal.Parse(el.Element(el.Name.Namespace + "Saldo")?.Value ?? "0", CultureInfo.InvariantCulture),
                Estado = el.Element(el.Name.Namespace + "Estado")?.Value ?? "",
                Moneda = el.Element(el.Name.Namespace + "Moneda")?.Value ?? "",
                EmailCliente = el.Element(el.Name.Namespace + "EmailCliente")?.Value ?? "",
                TelefonoCliente = el.Element(el.Name.Namespace + "TelefonoCliente")?.Value ?? ""
            });
        }
        return cuentas;
    }

    public async Task<List<MovimientoData>> ObtenerMovimientosAsync(string cuenta)
    {
        // Nombre exacto: GetMovimientos
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

        // WCF devuelve los objetos de la lista con el nombre de la clase: MovimientoData
        var elementos = doc.Descendants().Where(x => x.Name.LocalName == "MovimientoData").ToList();

        foreach (var el in elementos)
        {
            movimientos.Add(new MovimientoData
            {
                CodigoCuenta = el.Element(el.Name.Namespace + "CodigoCuenta")?.Value ?? "",
                Numero = int.Parse(el.Element(el.Name.Namespace + "Numero")?.Value ?? "0"),
                Fecha = DateTime.Parse(el.Element(el.Name.Namespace + "Fecha")?.Value ?? DateTime.Now.ToString("s"), CultureInfo.InvariantCulture),
                Tipo = el.Element(el.Name.Namespace + "Tipo")?.Value ?? "",
                Importe = decimal.Parse(el.Element(el.Name.Namespace + "Importe")?.Value ?? "0", CultureInfo.InvariantCulture),
                Referencia = el.Element(el.Name.Namespace + "Referencia")?.Value,
                SaldoActual = decimal.Parse(el.Element(el.Name.Namespace + "SaldoActual")?.Value ?? "0", CultureInfo.InvariantCulture),
                NombreCliente = el.Element(el.Name.Namespace + "NombreCliente")?.Value ?? ""
            });
        }
        return movimientos;
    }

    public async Task<ResultadoOperacion> RegistrarMovimientoAsync(string cuentaOrigen, string cuentaDestino, string tipo, double importe)
    {
        string destino = string.IsNullOrEmpty(cuentaDestino) ? "" : cuentaDestino;
        string importeStr = importe.ToString(CultureInfo.InvariantCulture);

        // OJO: Tu método en C# recibe (string tipo, string cuenta, string cuentaDestino, decimal monto)
        // Pero en el WSDL se llaman (tipo, cuenta, cuentaDestino, monto)
        // Para evitar problemas, enviamos los tags con los nombres exactos del WSDL
        string soapXml = $@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:tem=""http://tempuri.org/"">
                               <soapenv:Header/>
                               <soapenv:Body>
                                  <tem:ProcesarMovimiento>
                                     <tem:tipo>{tipo}</tem:tipo>
                                     <tem:cuenta>{cuentaOrigen}</tem:cuenta>
                                     <tem:cuentaDestino>{destino}</tem:cuentaDestino>
                                     <tem:monto>{importeStr}</tem:monto>
                                  </tem:ProcesarMovimiento>
                               </soapenv:Body>
                            </soapenv:Envelope>";

        var response = await EnviarSoap(ConstantesRed.UrlDotNetSoapMovimientos, soapXml, "http://tempuri.org/IMovimientosService/ProcesarMovimiento");
        var doc = XDocument.Parse(response);

        var resultadoEl = doc.Descendants().FirstOrDefault(x => x.Name.LocalName == "ResultadoOperacion");

        if (resultadoEl == null)
            return new ResultadoOperacion { Codigo = -1, Mensaje = "Error al procesar la respuesta WCF." };

        return new ResultadoOperacion
        {
            Codigo = int.Parse(resultadoEl.Element(resultadoEl.Name.Namespace + "Codigo")?.Value ?? "-1"),
            Mensaje = resultadoEl.Element(resultadoEl.Name.Namespace + "Mensaje")?.Value ?? "Error desconocido"
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
}