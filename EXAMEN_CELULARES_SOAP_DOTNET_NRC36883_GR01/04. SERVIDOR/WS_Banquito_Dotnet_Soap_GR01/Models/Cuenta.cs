using System.Runtime.Serialization;

namespace WS_Banquito_Dotnet_Soap_GR01.Models
{
    [DataContract]
    public class Cuenta
    {
        [DataMember]
        public string NumCuenta { get; set; }

        [DataMember]
        public int CodCliente { get; set; }

        [DataMember]
        public decimal Saldo { get; set; }
    }
}