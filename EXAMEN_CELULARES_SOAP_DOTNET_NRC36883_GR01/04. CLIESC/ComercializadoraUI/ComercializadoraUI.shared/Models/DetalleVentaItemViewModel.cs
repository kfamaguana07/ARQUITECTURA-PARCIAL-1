namespace ComercializadoraUI.shared.Models
{
    public class DetalleVentaItemViewModel
    {
        public int ProductoId { get; set; }
        public string ProductoNombre { get; set; } = string.Empty;
        public decimal PrecioUnitario { get; set; }
        public int Cantidad { get; set; } = 1;
        public decimal Subtotal => PrecioUnitario * Cantidad;
    }
}