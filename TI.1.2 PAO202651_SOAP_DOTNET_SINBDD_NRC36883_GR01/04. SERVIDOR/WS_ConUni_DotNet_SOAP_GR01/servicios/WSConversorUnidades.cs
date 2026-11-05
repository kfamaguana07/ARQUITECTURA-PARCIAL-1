using System.ServiceModel;
using ec.edu.monster.Controladores;

namespace ec.edu.monster.Servicios
{
    // Implementación del contrato SOAP. Delega la lógica a los controladores.
    public class WSConversorUnidades : IWSConversorUnidades
    {
        private readonly ConversionesControlador conversiones = new ConversionesControlador();
        private readonly SeguridadControlador seguridad = new SeguridadControlador();

        public string Login(string usuario, string contrasenia)
        {
            return seguridad.Login(usuario, contrasenia);
        }

        public bool CambiarContrasenia(string contraseniaActual, string nuevaContrasenia)
        {
            return seguridad.CambiarContrasenia(contraseniaActual, nuevaContrasenia);
        }

        public double ConvertirLongitud(double valor, string unidadInicial, string unidadFinal)
        {
            return conversiones.ConvertirLongitud(valor, unidadInicial, unidadFinal);
        }

        public double ConvertirMasa(double valor, string unidadInicial, string unidadFinal)
        {
            return conversiones.ConvertirMasa(valor, unidadInicial, unidadFinal);
        }

        public double ConvertirTemperatura(double valor, string unidadInicial, string unidadFinal)
        {
            return conversiones.ConvertirTemperatura(valor, unidadInicial, unidadFinal);
        }
    }
}
