namespace ec.edu.monster.Modelos;

public class MovimientoData
{
    public string CodigoCuenta { get; set; } = string.Empty;
    public int Numero { get; set; }
    public DateTime Fecha { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public decimal Importe { get; set; }
    public string Referencia { get; set; } = string.Empty;
    public decimal SaldoActual { get; set; }
    public string NombreCliente { get; set; } = string.Empty;
}