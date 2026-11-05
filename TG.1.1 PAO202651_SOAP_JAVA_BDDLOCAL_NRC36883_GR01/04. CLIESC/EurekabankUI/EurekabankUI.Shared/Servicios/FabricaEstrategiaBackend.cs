using ec.edu.monster.Interfaces;
using ec.edu.monster.Modelos;

namespace ec.edu.monster.Servicios;

public class FabricaEstrategiaBackend
{
    private readonly IEnumerable<IAdaptadorBanco> _adaptadoresBanco;
    private readonly IEnumerable<IAdaptadorAutenticacion> _adaptadoresAutenticacion;

    public FabricaEstrategiaBackend(
        IEnumerable<IAdaptadorBanco> adaptadoresBanco,
        IEnumerable<IAdaptadorAutenticacion> adaptadoresAutenticacion)
    {
        _adaptadoresBanco = adaptadoresBanco;
        _adaptadoresAutenticacion = adaptadoresAutenticacion;
    }

    public IAdaptadorBanco ObtenerAdaptadorBanco(TipoBackend tipo) =>
        _adaptadoresBanco.FirstOrDefault(a => a.Tipo == tipo)
        ?? throw new NotSupportedException($"Adaptador de banco para {tipo} no implementado.");

    public IAdaptadorAutenticacion ObtenerAdaptadorAutenticacion(TipoBackend tipo) =>
        _adaptadoresAutenticacion.FirstOrDefault(a => a.Tipo == tipo)
        ?? throw new NotSupportedException($"Adaptador de autenticación para {tipo} no implementado.");
}