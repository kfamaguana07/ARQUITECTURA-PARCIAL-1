namespace ec.edu.monster.Modelos;

public class DatosCuenta
{
    public string Codigo { get; set; } = string.Empty;
    public string NombreCliente { get; set; } = string.Empty;
    public decimal Saldo { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string Moneda { get; set; } = string.Empty;
    public string EmailCliente { get; set; } = string.Empty;
    public string TelefonoCliente { get; set; } = string.Empty;
}