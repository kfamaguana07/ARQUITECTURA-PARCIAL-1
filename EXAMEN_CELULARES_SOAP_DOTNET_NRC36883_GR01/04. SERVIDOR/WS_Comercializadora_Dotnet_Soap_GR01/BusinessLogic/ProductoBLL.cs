using System.Collections.Generic;
using WS_Comercializadora_Dotnet_Soap_GR01.DataAccess;
using WS_Comercializadora_Dotnet_Soap_GR01.Models;

namespace WS_Comercializadora_Dotnet_Soap_GR01.BusinessLogic
{
    public class ProductoBLL
    {
        private readonly ProductoDAL _productoDAL = new ProductoDAL();

        public List<Producto> ObtenerCatalogo()
        {
            return _productoDAL.ObtenerTodos();
        }

        public bool Guardar(Producto producto)
        {
            if (string.IsNullOrWhiteSpace(producto.ProNombre) || producto.ProPrecio <= 0)
                return false;

            return _productoDAL.Insertar(producto);
        }

        public bool Actualizar(Producto producto)
        {
            if (producto.ProId <= 0 || string.IsNullOrWhiteSpace(producto.ProNombre) || producto.ProPrecio <= 0)
                return false;

            return _productoDAL.Actualizar(producto);
        }

        public bool Eliminar(int id)
        {
            if (id <= 0) return false;
            return _productoDAL.Eliminar(id);
        }
    }
}