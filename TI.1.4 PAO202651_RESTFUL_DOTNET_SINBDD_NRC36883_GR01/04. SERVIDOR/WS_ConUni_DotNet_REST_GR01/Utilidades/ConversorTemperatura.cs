using WS_ConUni_DotNet_Rest_GR5.Modelos.Utilidades.Enums;

namespace WS_ConUni_DotNet_Rest_GR5.Modelos.Utilidades;

public class ConversorTemperatura : IConversor<UnidadTemperatura>
{
    private readonly Dictionary<UnidadTemperatura, (double Factor, double Offset)> _factoresKelvin = new()
    {
        { UnidadTemperatura.Celsius, (1.0, 273.15) },
        { UnidadTemperatura.Fahrenheit, (5.0 / 9.0, 459.67) },
        { UnidadTemperatura.Kelvin, (1.0, 0.0) },
        { UnidadTemperatura.Rankine, (5.0 / 9.0, 0.0) }
    };

    public double Convertir(double valor, UnidadTemperatura unidadOrigen, UnidadTemperatura unidadFinal)
    {
        if (!_factoresKelvin.ContainsKey(unidadOrigen) || !_factoresKelvin.ContainsKey(unidadFinal))
        {
            throw new ArgumentException("Unidad no soportada");
        }

        if (unidadOrigen == unidadFinal)
        {
            return valor;
        }

        var origen = _factoresKelvin[unidadOrigen];
        var destino = _factoresKelvin[unidadFinal];

        var valorEnKelvin = (valor + origen.Offset) * origen.Factor;
        return (valorEnKelvin / destino.Factor) - destino.Offset;
    }
}
