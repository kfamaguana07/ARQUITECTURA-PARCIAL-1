using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WS_Eurekabank_Dotnet_Soap_GR01.Models
{
    [Table("TipoMovimiento")]
    public class TipoMovimiento
    {
        [Key]
        [Column("chr_tipocodigo")]
        public string ChrTipoCodigo { get; set; }

        [Column("vch_tipodescripcion")]
        public string VchTipoDescripcion { get; set; }

        [Column("vch_tipoaccion")]
        public string VchTipoAccion { get; set; }

        [Column("vch_tipoestado")]
        public string VchTipoEstado { get; set; }
    }
}