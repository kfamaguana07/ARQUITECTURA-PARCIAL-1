namespace Eurekabank_ClienteConsola_Dotnet_GR01.Models;

public class CrearMovimientoRequest
{
    public string? Tipo { get; set; }
    public string? CuentaOrigen { get; set; }
    public string? CuentaDestino { get; set; }
    public decimal Importe { get; set; }
}