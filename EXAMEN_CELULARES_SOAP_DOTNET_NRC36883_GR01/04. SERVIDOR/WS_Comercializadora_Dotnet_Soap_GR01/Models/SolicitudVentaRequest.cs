using System.Runtime.Serialization;

namespace WS_Comercializadora_Dotnet_Soap_GR01.Models
{
    [DataContract]
    public class SolicitudVentaRequest
    {
        [DataMember]
        public string CedulaCliente { get; set; }

        [DataMember]
        public int ProductoId { get; set; }

        [DataMember]
        public string FormaPago { get; set; } // "E" = Efectivo, "C" = Crédito Directo

        [DataMember]
        public int NumeroCuotas { get; set; } // Requerido si FormaPago == "C" (entre 3 y 18)
    }
}