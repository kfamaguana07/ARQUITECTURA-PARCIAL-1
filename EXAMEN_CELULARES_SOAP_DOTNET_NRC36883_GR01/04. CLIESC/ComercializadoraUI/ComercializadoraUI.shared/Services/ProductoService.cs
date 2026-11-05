using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComercializadoraServiceReference;
using ComercializadoraUI.shared.Helpers;
using ComercializadoraUI.shared.Interfaces;

namespace ComercializadoraUI.shared.Services
{
    public class ProductoService : IProductoService
    {
        private readonly ComercializadoraServiceClient _client;

        public ProductoService(ComercializadoraServiceClient client)
        {
            _client = client;
        }

        public async Task<List<Producto>> ObtenerCatalogoAsync()
        {
            try
            {
                var resultado = await _client.ObtenerCatalogoProductosAsync();
                return resultado != null ? resultado.ToList() : new List<Producto>();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ExceptionHelper.ObtenerMensajeAmigable(ex));
            }
        }

        public async Task<bool> GuardarProductoAsync(Producto producto)
        {
            try
            {
                return await _client.GuardarProductoAsync(producto);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ExceptionHelper.ObtenerMensajeAmigable(ex));
            }
        }

        public async Task<bool> ActualizarProductoAsync(Producto producto)
        {
            try
            {
                return await _client.ActualizarProductoAsync(producto);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ExceptionHelper.ObtenerMensajeAmigable(ex));
            }
        }

        public async Task<bool> EliminarProductoAsync(int productoId)
        {
            try
            {
                return await _client.EliminarProductoAsync(productoId);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ExceptionHelper.ObtenerMensajeAmigable(ex));
            }
        }
    }
}