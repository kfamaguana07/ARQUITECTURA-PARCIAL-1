using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WS_Eurekabank_Dotnet_Soap_GR01.Models
{
    [Table("Usuario")]
    public class Usuario
    {
        [Key]
        [Column("chr_emplcodigo")]
        public string ChrEmplCodigo { get; set; }

        [Column("vch_emplusuario")]
        public string VchEmplUsuario { get; set; }

        [Column("vch_emplclave")]
        public string VchEmplClave { get; set; }

        [Column("vch_emplestado")]
        public string VchEmplEstado { get; set; }
    }
}