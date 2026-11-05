using System.Collections.Generic;
using System.Runtime.Serialization;
using WS_Comercializadora_Dotnet_Soap_GR01.BanquitoServiceReference;

namespace WS_Comercializadora_Dotnet_Soap_GR01.Models
{
    [DataContract]
    public class RespuestaVenta
    {
        [DataMember]
        public bool Exitosa { get; set; }

        [DataMember]
        public string Mensaje { get; set; }

        [DataMember]
        public int FacturaId { get; set; }

        [DataMember]
        public decimal TotalPagar { get; set; }

        [DataMember]
        public int? CreditoId { get; set; }

        [DataMember]
        public List<CuotaAmortizacion> TablaAmortizacion { get; set; }
    }
}