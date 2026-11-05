using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WS_Banquito_Dotnet_Soap_GR01.Models
{
    [DataContract]
    public class Credito
    {
        [DataMember]
        public int CodCredito { get; set; }

        [DataMember]
        public int CodCliente { get; set; }

        [DataMember]
        public decimal Monto { get; set; }

        [DataMember]
        public int PlazoMeses { get; set; }

        [DataMember]
        public decimal TasaInteres { get; set; }

        [DataMember]
        public DateTime FechaCreacion { get; set; }

        [DataMember]
        public List<CuotaAmortizacion> TablaAmortizacion { get; set; }

        public Credito()
        {
            TablaAmortizacion = new List<CuotaAmortizacion>();
        }
    }
}