namespace WS_Eurekabank_Dotnet_Rest_GR01.DTOs
{
    public class MovimientoRequestDTO
    {
        public string CuentaOrigen { get; set; }
        public string CuentaDestino { get; set; }
        public string Tipo { get; set; } // DEPOSITO, RETIRO, TRANSFERENCIA
        public decimal Importe { get; set; }
    }
}
