using System.Runtime.Serialization;

namespace WS_Comercializadora_Dotnet_Soap_GR01.Models
{
    [DataContract]
    public class DetalleVentaItemContract
    {
        [DataMember]
        public int ProductoId { get; set; }

        [DataMember]
        public int Cantidad { get; set; }
    }
}