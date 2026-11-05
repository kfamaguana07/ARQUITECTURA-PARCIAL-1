using ComercializadoraServiceReference;

namespace ComercializadoraUI.shared.Interfaces
{
    public interface IAmortizacionService
    {
        Task<List<CuotaAmortizacion>> ConsultarAmortizacionAsync(int codigoCredito);
    }
}