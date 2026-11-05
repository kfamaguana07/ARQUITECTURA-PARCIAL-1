using System.Runtime.Serialization;

namespace WS_Banquito_Dotnet_Soap_GR01.Models
{
    [DataContract]
    public class CuotaAmortizacion
    {
        [DataMember]
        public int NumCuota { get; set; }

        [DataMember]
        public decimal ValorCuota { get; set; }

        [DataMember]
        public decimal InteresPagado { get; set; }

        [DataMember]
        public decimal CapitalPagado { get; set; }

        [DataMember]
        public decimal SaldoRestante { get; set; }
    }
}