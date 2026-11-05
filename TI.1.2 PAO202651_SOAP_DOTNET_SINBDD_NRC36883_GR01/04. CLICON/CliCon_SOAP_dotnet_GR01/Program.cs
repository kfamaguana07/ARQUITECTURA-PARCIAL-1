using ClICON.Modelos;
using ClICON.Controladores;
using ClICON.Vistas;
using System.Threading.Tasks;

namespace ClICON
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ConversorModelo modelo = new ConversorModelo();
            ConsolaVista vista = new ConsolaVista();
            AppControlador controlador = new AppControlador(modelo, vista);

            await controlador.IniciarAppAsync();
        }
    }
}