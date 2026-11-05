using System.Runtime.Serialization;

namespace WS_Banquito_Dotnet_Soap_GR01.Models
{
    [DataContract]
    public class RespuestaValidacion
    {
        [DataMember]
        public bool EsSujeto { get; set; }

        [DataMember]
        public int CodigoCliente { get; set; }

        [DataMember]
        public string Mensaje { get; set; }
    }
}