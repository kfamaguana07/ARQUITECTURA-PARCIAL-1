using ec.edu.monster.Modelos;

namespace ec.edu.monster.Interfaces;

public interface IAdaptadorBanco
{
    TipoBackend Tipo { get; }
    Task<List<DatosCuenta>> ObtenerCuentasAsync();
    Task<List<MovimientoData>> ObtenerMovimientosAsync(string cuenta);
    Task<ResultadoOperacion> RegistrarMovimientoAsync(string cuentaOrigen, string cuentaDestino, string tipo, double importe);
}