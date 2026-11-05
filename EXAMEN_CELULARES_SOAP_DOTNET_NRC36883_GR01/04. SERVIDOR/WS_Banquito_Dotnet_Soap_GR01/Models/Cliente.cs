using System;
using System.Runtime.Serialization;

namespace WS_Banquito_Dotnet_Soap_GR01.Models
{
    [DataContract]
    public class Cliente
    {
        [DataMember]
        public int CodCliente { get; set; }

        [DataMember]
        public string Cedula { get; set; }

        [DataMember]
        public string Nombre { get; set; }

        [DataMember]
        public string Genero { get; set; }

        [DataMember]
        public DateTime FechaNacimiento { get; set; }
    }
}