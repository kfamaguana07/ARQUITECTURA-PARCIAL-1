using System;

namespace Comercializadora_ClienteConsola_Dotnet_GR01.Views
{
    public class MenuView
    {
        public int MostrarMenuPrincipal()
        {
            ConsoleHelper.MostrarTitulo("COMERCIALIZADORA DE TELÉFONOS CELULARES - MENÚ PRINCIPAL");
            Console.WriteLine("1. Mantenimiento del Catálogo de Productos (CRUD)");
            Console.WriteLine("2. Procesar Venta de Teléfono (Efectivo / Crédito Directo)");
            Console.WriteLine("3. Consultar Tabla de Amortización por Crédito");
            Console.WriteLine("0. Salir del Sistema");
            Console.WriteLine("--------------------------------------------------------------------------------");

            return ConsoleHelper.LeerEntero("Seleccione una opción", 0, 3);
        }

        public int MostrarSubMenuProductos()
        {
            ConsoleHelper.MostrarTitulo("GESTIÓN DE CATÁLOGO DE PRODUCTOS");
            Console.WriteLine("1. Listar Catálogo de Productos");
            Console.WriteLine("2. Registrar Nuevo Producto");
            Console.WriteLine("3. Actualizar Producto Existente");
            Console.WriteLine("4. Eliminar Producto");
            Console.WriteLine("0. Volver al Menú Principal");
            Console.WriteLine("--------------------------------------------------------------------------------");

            return ConsoleHelper.LeerEntero("Seleccione una opción", 0, 4);
        }
    }
}