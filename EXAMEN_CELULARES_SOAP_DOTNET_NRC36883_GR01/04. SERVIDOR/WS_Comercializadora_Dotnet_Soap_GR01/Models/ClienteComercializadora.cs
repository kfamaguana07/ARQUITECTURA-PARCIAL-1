using System.Runtime.Serialization;

namespace WS_Comercializadora_Dotnet_Soap_GR01.Models
{
    [DataContract]
    public class ClienteComercializadora
    {
        [DataMember]
        public int CliId { get; set; }

        [DataMember]
        public string CliCedula { get; set; }

        [DataMember]
        public string CliNombre { get; set; }

        [DataMember]
        public string CliApellido { get; set; }

        [DataMember]
        public string CliTelefono { get; set; }

        [DataMember]
        public string CliDireccion { get; set; }
    }
}