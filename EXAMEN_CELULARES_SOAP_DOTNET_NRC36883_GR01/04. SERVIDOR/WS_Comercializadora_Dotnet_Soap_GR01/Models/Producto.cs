using System.Runtime.Serialization;

namespace WS_Comercializadora_Dotnet_Soap_GR01.Models
{
    [DataContract]
    public class Producto
    {
        [DataMember]
        public int ProId { get; set; }

        [DataMember]
        public string ProNombre { get; set; }

        [DataMember]
        public string ProDescripcion { get; set; }

        [DataMember]
        public decimal ProPrecio { get; set; }
    }
}