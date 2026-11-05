using WS_ConUni_DotNet_Rest_GR5.Modelos.Utilidades;
using WS_ConUni_DotNet_Rest_GR5.Modelos.Utilidades.Enums;

namespace WS_ConUni_DotNet_Rest_GR5.Modelos.Servicios;

public class ServicioConversion
{
    private readonly ConversorLongitud _conversorLongitud;
    private readonly ConversorTemperatura _conversorTemperatura;
    private readonly ConversorMasa _conversorMasa;

    public ServicioConversion(
        ConversorLongitud conversorLongitud,
        ConversorTemperatura conversorTemperatura,
        ConversorMasa conversorMasa)
    {
        _conversorLongitud = conversorLongitud;
        _conversorTemperatura = conversorTemperatura;
        _conversorMasa = conversorMasa;
    }

    public double ConvertirLongitud(double valor, string unidadOrigen, string unidadDestino)
    {
        var origen = ParseUnidad<UnidadLongitud>(unidadOrigen);
        var destino = ParseUnidad<UnidadLongitud>(unidadDestino);
        return _conversorLongitud.Convertir(valor, origen, destino);
    }

    public double ConvertirTemperatura(double valor, string unidadOrigen, string unidadDestino)
    {
        var origen = ParseUnidad<UnidadTemperatura>(unidadOrigen);
        var destino = ParseUnidad<UnidadTemperatura>(unidadDestino);
        return _conversorTemperatura.Convertir(valor, origen, destino);
    }

    public double ConvertirMasa(double valor, string unidadOrigen, string unidadDestino)
    {
        var origen = ParseUnidad<UnidadMasa>(unidadOrigen);
        var destino = ParseUnidad<UnidadMasa>(unidadDestino);
        return _conversorMasa.Convertir(valor, origen, destino);
    }

    private static TUnidad ParseUnidad<TUnidad>(string valorUnidad) where TUnidad : struct, Enum
    {
        if (!Enum.TryParse<TUnidad>(valorUnidad, true, out var unidad))
        {
            throw new ArgumentException($"Unidad no soportada: {valorUnidad}");
        }

        return unidad;
    }
}
