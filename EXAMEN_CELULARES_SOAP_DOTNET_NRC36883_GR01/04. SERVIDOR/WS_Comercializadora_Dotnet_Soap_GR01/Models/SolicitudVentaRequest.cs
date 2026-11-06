using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WS_Comercializadora_Dotnet_Soap_GR01.Models
{
    [DataContract]
    public class SolicitudVentaRequest
    {
        [DataMember]
        public string CedulaCliente { get; set; }

        [DataMember]
        public string FormaPago { get; set; } // 'E' (Efectivo) o 'C' (Crédito)

        [DataMember]
        public int NumeroCuotas { get; set; }

        [DataMember]
        public List<DetalleVentaItemContract> Items { get; set; } = new List<DetalleVentaItemContract>();
    }
}