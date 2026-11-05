public class MovimientoDataDTO
{
    public string CodigoCuenta { get; set; }
    public int Numero { get; set; }
    public DateTime Fecha { get; set; }
    public string Tipo { get; set; }
    public decimal Importe { get; set; }
    public string? Referencia { get; set; }
    public decimal SaldoActual { get; set; }
    public string NombreCliente { get; set; }
}