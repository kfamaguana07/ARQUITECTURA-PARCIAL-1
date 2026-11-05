using System;
using System.Runtime.Serialization;

namespace WS_Banquito_Dotnet_Soap_GR01.Models
{
    [DataContract]
    public class Movimiento
    {
        [DataMember]
        public int CodMovimiento { get; set; }

        [DataMember]
        public string NumCuenta { get; set; }

        [DataMember]
        public string Tipo { get; set; } // 'DEP' o 'RET'

        [DataMember]
        public decimal Valor { get; set; }

        [DataMember]
        public DateTime Fecha { get; set; }
    }
}