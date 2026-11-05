using System.ComponentModel.DataAnnotations;

namespace ComercializadoraUI.shared.Models
{
    public class SolicitudVentaViewModel
    {
        [Required(ErrorMessage = "La cédula del cliente es obligatoria.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "La cédula debe contener exactamente 10 dígitos.")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "La cédula debe contener únicamente números.")]
        public string CedulaCliente { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe seleccionar un producto del catálogo.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un producto válido.")]
        public int ProductoId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una forma de pago.")]
        public string FormaPago { get; set; } = "E"; // 'E' = Efectivo, 'C' = Crédito Directo

        [Range(3, 18, ErrorMessage = "El número de cuotas para crédito debe ser entre 3 y 18 meses.")]
        public int NumeroCuotas { get; set; } = 6;
    }
}