using System.ServiceModel;

namespace ec.edu.monster.Servicios
{
    [ServiceContract(Name = "WSConversorUnidades")]
    public interface IWSConversorUnidades
    {
        [OperationContract(Name = "login")]
        string Login(string usuario, string contrasenia);

        [OperationContract(Name = "cambiarContrasenia")]
        bool CambiarContrasenia(string contraseniaActual, string nuevaContrasenia);

        [OperationContract(Name = "convertirLongitud")]
        double ConvertirLongitud(double valor, string unidadInicial, string unidadFinal);

        [OperationContract(Name = "convertirMasa")]
        double ConvertirMasa(double valor, string unidadInicial, string unidadFinal);

        [OperationContract(Name = "convertirTemperatura")]
        double ConvertirTemperatura(double valor, string unidadInicial, string unidadFinal);
    }
}

