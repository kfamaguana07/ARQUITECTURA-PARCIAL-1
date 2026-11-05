using ComercializadoraServiceReference;

namespace Comercializadora_ClienteConsola_Dotnet_GR01.Views
{
    public class ProductoView
    {
        public void MostrarProductos(IEnumerable<Producto> productos)
        {
            ConsoleHelper.MostrarTitulo("CATÁLOGO DE PRODUCTOS DISPONIBLES");
            Console.WriteLine(string.Format("{0,-6} | {1,-25} | {2,-30} | {3,10}", "ID", "NOMBRE", "DESCRIPCIÓN", "PRECIO ($)"));
            Console.WriteLine(new string('-', 78));

            int contador = 0;
            foreach (var p in productos)
            {
                string desc = p.ProDescripcion != null && p.ProDescripcion.Length > 28
                    ? p.ProDescripcion.Substring(0, 25) + "..."
                    : (p.ProDescripcion ?? "");

                Console.WriteLine(string.Format("{0,-6} | {1,-25} | {2,-30} | {3,10:N2}", p.ProId, p.ProNombre, desc, p.ProPrecio));
                contador++;
            }

            if (contador == 0)
            {
                ConsoleHelper.MostrarMensajeInfo("No existen productos registrados en el catálogo.");
            }
            Console.WriteLine(new string('-', 78));
        }

        public Producto CapturarNuevoProducto()
        {
            ConsoleHelper.MostrarTitulo("REGISTRAR NUEVO TELÉFONO CELULAR");
            return new Producto
            {
                ProNombre = ConsoleHelper.LeerTextoRequerido("Nombre / Modelo del Producto"),
                ProDescripcion = ConsoleHelper.LeerTextoRequerido("Descripción / Especificaciones"),
                ProPrecio = ConsoleHelper.LeerDecimal("Precio de Venta ($)")
            };
        }

        public Producto CapturarActualizacionProducto()
        {
            ConsoleHelper.MostrarTitulo("ACTUALIZAR DATOS DE PRODUCTO");
            return new Producto
            {
                ProId = ConsoleHelper.LeerEntero("Ingrese el ID del Producto a Modificar", 1),
                ProNombre = ConsoleHelper.LeerTextoRequerido("Nuevo Nombre / Modelo"),
                ProDescripcion = ConsoleHelper.LeerTextoRequerido("Nueva Descripción"),
                ProPrecio = ConsoleHelper.LeerDecimal("Nuevo Precio ($)")
            };
        }

        public int CapturarIdEliminar()
        {
            ConsoleHelper.MostrarTitulo("ELIMINAR PRODUCTO DEL CATÁLOGO");
            return ConsoleHelper.LeerEntero("Ingrese el ID del Producto a Eliminar", 1);
        }
    }
}