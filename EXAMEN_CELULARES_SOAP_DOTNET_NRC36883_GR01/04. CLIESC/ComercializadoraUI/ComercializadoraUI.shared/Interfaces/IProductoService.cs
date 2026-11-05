using ComercializadoraServiceReference;

namespace ComercializadoraUI.shared.Interfaces
{
    public interface IProductoService
    {
        Task<List<Producto>> ObtenerCatalogoAsync();
        Task<bool> GuardarProductoAsync(Producto producto);
        Task<bool> ActualizarProductoAsync(Producto producto);
        Task<bool> EliminarProductoAsync(int productoId);
    }
}