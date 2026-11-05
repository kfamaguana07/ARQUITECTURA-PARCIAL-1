using System.ServiceModel;
using ec.edu.monster.controladores;
using ec.edu.monster.servicios;

namespace ec.edu.monster
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class WSConversorUnidades : IWSConversorUnidades
    {
        private readonly SeguridadControlador seguridadControlador = new SeguridadControlador();
        private readonly ConversionesControlador conversionesControlador = new ConversionesControlador();

        public string Login(string usuario, string contrasenia)
        {
            var token = seguridadControlador.Login(usuario, contrasenia);
            if (!string.IsNullOrWhiteSpace(token))
            {
                return token;
            }

            throw new FaultException("Credenciales incorrectas");
        }

        public bool CambiarContrasenia(string contraseniaActual, string nuevaContrasenia)
        {
            if (seguridadControlador.CambiarContrasenia(contraseniaActual, nuevaContrasenia))
            {
                return true;
            }

            throw new FaultException("No se pudo cambiar la contrasenia");
        }

        public double ConvertirLongitud(double valor, string unidadInicial, string unidadFinal)
        {
            return conversionesControlador.ConvertirLongitud(valor, unidadInicial, unidadFinal);
        }

        public double ConvertirMasa(double valor, string unidadInicial, string unidadFinal)
        {
            return conversionesControlador.ConvertirMasa(valor, unidadInicial, unidadFinal);
        }

        public double ConvertirTemperatura(double valor, string unidadInicial, string unidadFinal)
        {
            return conversionesControlador.ConvertirTemperatura(valor, unidadInicial, unidadFinal);
        }
    }
}

