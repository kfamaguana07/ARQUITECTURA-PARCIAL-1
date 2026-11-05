namespace WS_Eurekabank_Dotnet_Rest_GR01.DTOs
{
    public class ResultadoOperacionDTO
    {
        public int Codigo { get; set; }
        public string Mensaje { get; set; }

        public static ResultadoOperacionDTO Exito(string msg = "Operación exitosa") => new() { Codigo = 1, Mensaje = msg };
        public static ResultadoOperacionDTO Error(string msg) => new() { Codigo = -1, Mensaje = msg };
    }
}
