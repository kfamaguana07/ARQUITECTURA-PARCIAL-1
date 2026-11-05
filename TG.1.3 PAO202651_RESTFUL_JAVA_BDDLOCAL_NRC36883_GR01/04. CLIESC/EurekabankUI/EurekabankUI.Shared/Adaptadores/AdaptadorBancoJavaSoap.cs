using System.Globalization;
using System.Text;
using System.Xml.Linq;
using ec.edu.monster.Interfaces;
using ec.edu.monster.Modelos;

namespace ec.edu.monster.Adaptadores;

public class AdaptadorBancoJavaSoap : IAdaptadorBanco
{
    public TipoBackend Tipo => TipoBackend.JavaSoap;
    private readonly HttpClient _httpClient;

    public AdaptadorBancoJavaSoap(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // ═══════════════════════════════════════════════════════════════
    // 1. TRAER CUENTAS
    // ═══════════════════════════════════════════════════════════════
    public async Task<List<DatosCuenta>> ObtenerCuentasAsync()
    {
        string soapXml = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ws=""http://ws.monster.edu.ec/"">
                               <soapenv:Header/>
                               <soapenv:Body>
                                  <ws:traerCuentasConClientes/>
                               </soapenv:Body>
                            </soapenv:Envelope>";

        var response = await EnviarSoap(soapXml);
        var doc = XDocument.Parse(response);

        var cuentas = new List<DatosCuenta>();

        // 🔍 Buscamos la etiqueta que definiste en @WebResult(name = "GetDatosCuentasResult")
        var elementos = doc.Descendants().Where(x => x.Name.LocalName == "GetDatosCuentasResult").ToList();

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

    // ═══════════════════════════════════════════════════════════════
    // 2. TRAER MOVIMIENTOS
    // ═══════════════════════════════════════════════════════════════
    public async Task<List<MovimientoData>> ObtenerMovimientosAsync(string cuenta)
    {
        string soapXml = $@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ws=""http://ws.monster.edu.ec/"">
                               <soapenv:Header/>
                               <soapenv:Body>
                                  <ws:traerMovimientos>
                                     <cuenta>{cuenta}</cuenta>
                                  </ws:traerMovimientos>
                               </soapenv:Body>
                            </soapenv:Envelope>";

        var response = await EnviarSoap(soapXml);
        var doc = XDocument.Parse(response);

        var movimientos = new List<MovimientoData>();

        // 🔍 Buscamos la etiqueta que definiste en @WebResult(name = "MovimientoData")
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

    // ═══════════════════════════════════════════════════════════════
    // 3. REGISTRAR MOVIMIENTO
    // ═══════════════════════════════════════════════════════════════
    public async Task<ResultadoOperacion> RegistrarMovimientoAsync(string cuentaOrigen, string cuentaDestino, string tipo, double importe)
    {
        // Si es depósito o retiro, la cuenta destino va vacía
        string destinoXml = string.IsNullOrEmpty(cuentaDestino) ? "" : cuentaDestino;

        // Formateamos el importe con punto decimal (InvariantCulture)
        string importeFormateado = importe.ToString(CultureInfo.InvariantCulture);

        string soapXml = $@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ws=""http://ws.monster.edu.ec/"">
                               <soapenv:Header/>
                               <soapenv:Body>
                                  <ws:regMovimiento>
                                     <cuentaOrigen>{cuentaOrigen}</cuentaOrigen>
                                     <cuentaDestino>{destinoXml}</cuentaDestino>
                                     <tipo>{tipo}</tipo>
                                     <importe>{importeFormateado}</importe>
                                  </ws:regMovimiento>
                               </soapenv:Body>
                            </soapenv:Envelope>";

        var response = await EnviarSoap(soapXml);
        var doc = XDocument.Parse(response);

        // 🔍 Buscamos la etiqueta "resultado" (definida en @WebResult(name = "resultado"))
        var resultado = doc.Descendants().FirstOrDefault(x => x.Name.LocalName == "resultado");

        if (resultado == null)
            throw new Exception("No se recibió respuesta válida del servidor.");

        return new ResultadoOperacion
        {
            Codigo = int.Parse(resultado.Element(resultado.Name.Namespace + "codigo")?.Value ?? "-1"),
            Mensaje = resultado.Element(resultado.Name.Namespace + "mensaje")?.Value ?? "Error desconocido"
        };
    }

    // ═══════════════════════════════════════════════════════════════
    // MÉTODO AUXILIAR PARA ENVIAR SOAP
    // ═══════════════════════════════════════════════════════════════
    private async Task<string> EnviarSoap(string soapXml)
    {
        var content = new StringContent(soapXml, Encoding.UTF8, "text/xml");
        var response = await _httpClient.PostAsync(ConstantesRed.UrlJavaSoap, content);
        var responseXml = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            try
            {
                var docFault = XDocument.Parse(responseXml);
                var faultString = docFault.Descendants().FirstOrDefault(x => x.Name.LocalName == "faultstring")?.Value;
                throw new Exception($"Error SOAP Java: {faultString ?? response.StatusCode.ToString()}");
            }
            catch (System.Xml.XmlException)
            {
                throw new Exception($"Error HTTP: {response.StatusCode}");
            }
        }

        return responseXml;
    }
}