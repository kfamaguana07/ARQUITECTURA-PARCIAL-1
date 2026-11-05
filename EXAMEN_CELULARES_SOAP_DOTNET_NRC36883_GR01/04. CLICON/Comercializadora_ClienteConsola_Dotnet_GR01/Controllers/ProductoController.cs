using Comercializadora_ClienteConsola_Dotnet_GR01.Views;
using ComercializadoraServiceReference;
using System;

namespace Comercializadora_ClienteConsola_Dotnet_GR01.Controllers
{
    public class ProductoController
    {
        private readonly ProductoView _productoView = new ProductoView();

        public void EjecutarFlujo()
        {
            MenuView menuView = new MenuView();
            bool regresar = false;

            while (!regresar)
            {
                int opcion = menuView.MostrarSubMenuProductos();
                switch (opcion)
                {
                    case 1:
                        ListarProductos();
                        ConsoleHelper.Pausar();
                        break;
                    case 2:
                        RegistrarProducto();
                        ConsoleHelper.Pausar();
                        break;
                    case 3:
                        ActualizarProducto();
                        ConsoleHelper.Pausar();
                        break;
                    case 4:
                        EliminarProducto();
                        ConsoleHelper.Pausar();
                        break;
                    case 0:
                        regresar = true;
                        break;
                }
            }
        }

        public void ListarProductos()
        {
            using (var client = new ComercializadoraServiceClient())
            {
                try
                {
                    var productos = client.ObtenerCatalogoProductos();
                    _productoView.MostrarProductos(productos);
                }
                catch (Exception ex)
                {
                    ConsoleHelper.MostrarMensajeError("Fallo al comunicarse con el servicio SOAP: " + ex.Message);
                }
            }
        }

        private void RegistrarProducto()
        {
            Producto nuevo = _productoView.CapturarNuevoProducto();
            using (var client = new ComercializadoraServiceClient())
            {
                try
                {
                    bool exito = client.GuardarProducto(nuevo);
                    if (exito)
                        ConsoleHelper.MostrarMensajeExito("Producto registrado correctamente en el catálogo.");
                    else
                        ConsoleHelper.MostrarMensajeError("No se pudo guardar el producto.");
                }
                catch (Exception ex)
                {
                    ConsoleHelper.MostrarMensajeError("Error WCF: " + ex.Message);
                }
            }
        }

        private void ActualizarProducto()
        {
            ListarProductos();
            Producto producto = _productoView.CapturarActualizacionProducto();

            using (var client = new ComercializadoraServiceClient())
            {
                try
                {
                    bool exito = client.ActualizarProducto(producto);
                    if (exito)
                        ConsoleHelper.MostrarMensajeExito("Producto actualizado correctamente.");
                    else
                        ConsoleHelper.MostrarMensajeError("No se encontró o no se pudo actualizar el producto especificado.");
                }
                catch (Exception ex)
                {
                    ConsoleHelper.MostrarMensajeError("Error WCF: " + ex.Message);
                }
            }
        }

        private void EliminarProducto()
        {
            ListarProductos();
            int id = _productoView.CapturarIdEliminar();

            using (var client = new ComercializadoraServiceClient())
            {
                try
                {
                    bool exito = client.EliminarProducto(id);
                    if (exito)
                        ConsoleHelper.MostrarMensajeExito("Producto eliminado exitosamente.");
                    else
                        ConsoleHelper.MostrarMensajeError("No se pudo eliminar el producto especificado.");
                }
                catch (Exception ex)
                {
                    ConsoleHelper.MostrarMensajeError("Error WCF: " + ex.Message);
                }
            }
        }
    }
}