namespace Eurekabank_ClienteConsola_Rest_GR01.Models
{
    public class LoginRequest
    {
        public string Usuario { get; set; }
        public string Contrasena { get; set; }
    }

    public class ResultadoOperacion
    {
        public int Codigo { get; set; }
        public string Mensaje { get; set; }
    }

    public class Cuenta
    {
        public string Codigo { get; set; }
        public string NombreCliente { get; set; }
        public decimal Saldo { get; set; }
        public string Estado { get; set; }
        public string Moneda { get; set; }
        public string EmailCliente { get; set; }
        public string TelefonoCliente { get; set; }
    }

    public class Movement
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

    public class MovimientoRequest
    {
        public string Tipo { get; set; }
        public string CuentaOrigen { get; set; }
        public string? CuentaDestino { get; set; }
        public decimal Importe { get; set; }
    }
}