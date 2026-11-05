using System.ComponentModel.DataAnnotations;

namespace ComercializadoraUI.shared.Models
{
    public class ProductoViewModel
    {
        public int ProId { get; set; }

        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres.")]
        public string ProNombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string ProDescripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.01, 10000.00, ErrorMessage = "El precio debe ser un valor mayor a $0.00.")]
        public decimal ProPrecio { get; set; }
    }
}