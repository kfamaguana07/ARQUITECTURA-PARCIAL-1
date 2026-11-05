using ec.edu.monster.Modelos;

namespace ec.edu.monster.Interfaces;

public interface IAdaptadorAutenticacion
{
    TipoBackend Tipo { get; }
    Task<string> IniciarSesionAsync(string usuario, string contrasenia); // Retorna Ok
}