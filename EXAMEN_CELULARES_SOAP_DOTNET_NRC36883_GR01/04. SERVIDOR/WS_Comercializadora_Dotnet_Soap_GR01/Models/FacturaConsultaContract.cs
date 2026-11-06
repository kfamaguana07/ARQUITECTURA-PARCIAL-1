using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WS_Comercializadora_Dotnet_Soap_GR01.Models
{
    [DataContract]
    public class FacturaConsultaContract
    {
        [DataMember]
        public int FacturaId { get; set; }

        [DataMember]
        public string CedulaCliente { get; set; } = string.Empty;

        [DataMember]
        public DateTime FechaVenta { get; set; }

        [DataMember]
        public decimal Subtotal { get; set; }

        [DataMember]
        public decimal Descuento { get; set; }

        [DataMember]
        public decimal Total { get; set; }

        [DataMember]
        public string FormaPago { get; set; } = string.Empty; // "E" o "C"

        [DataMember]
        public int? CreditoId { get; set; }

        [DataMember]
        public List<DetalleFacturaConsultaContract> Detalle { get; set; } = new List<DetalleFacturaConsultaContract>();
    }

    [DataContract]
    public class DetalleFacturaConsultaContract
    {
        [DataMember]
        public int ProductoId { get; set; }

        [DataMember]
        public string ProductoNombre { get; set; } = string.Empty;

        [DataMember]
        public int Cantidad { get; set; }

        [DataMember]
        public decimal PrecioUnitario { get; set; }

        [DataMember]
        public decimal Subtotal { get; set; }
    }
}