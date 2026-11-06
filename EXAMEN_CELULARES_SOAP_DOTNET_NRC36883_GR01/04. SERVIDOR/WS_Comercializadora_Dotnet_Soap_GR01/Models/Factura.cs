using System;
using System.Runtime.Serialization;

namespace WS_Comercializadora_Dotnet_Soap_GR01.Models
{
    [DataContract]
    public class Factura
    {
        [DataMember]
        public int FacId { get; set; }

        [DataMember]
        public int CliId { get; set; }

        [DataMember]
        public DateTime FacFecha { get; set; }

        [DataMember]
        public string FacFormaPago { get; set; } // 'E' (Efectivo) o 'C' (Crédito)

        [DataMember]
        public decimal FacSubtotal { get; set; }

        [DataMember]
        public decimal FacDescuento { get; set; }

        [DataMember]
        public decimal FacPrecioTotal { get; set; }

        [DataMember]
        public int? FacNumCuotas { get; set; }

        [DataMember]
        public decimal? FacPrecioCuota { get; set; }

        [DataMember]
        public int? CreId { get; set; }
    }
}