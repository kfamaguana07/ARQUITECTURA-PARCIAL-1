using WS_ConUni_DotNet_Rest_GR5.Modelos.Utilidades.Enums;

namespace WS_ConUni_DotNet_Rest_GR5.Modelos.Utilidades;

public class ConversorLongitud : IConversor<UnidadLongitud>
{
    private readonly Dictionary<UnidadLongitud, double> _factores = new()
    {
        { UnidadLongitud.Milimetro, 0.001 },
        { UnidadLongitud.Centimetro, 0.01 },
        { UnidadLongitud.Metro, 1.0 },
        { UnidadLongitud.Kilometro, 1000.0 },
        { UnidadLongitud.Yarda, 0.9144 }
    };

    public double Convertir(double valor, UnidadLongitud unidadOrigen, UnidadLongitud unidadFinal)
    {
        if (!_factores.ContainsKey(unidadOrigen) || !_factores.ContainsKey(unidadFinal))
        {
            throw new ArgumentException("Unidad no soportada");
        }

        var valorEnBase = valor * _factores[unidadOrigen];
        return valorEnBase / _factores[unidadFinal];
    }
}
