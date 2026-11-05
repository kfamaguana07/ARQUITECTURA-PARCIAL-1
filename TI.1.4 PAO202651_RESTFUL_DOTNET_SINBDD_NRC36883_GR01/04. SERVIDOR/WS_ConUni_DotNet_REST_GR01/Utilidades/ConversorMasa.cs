using WS_ConUni_DotNet_Rest_GR5.Modelos.Utilidades.Enums;

namespace WS_ConUni_DotNet_Rest_GR5.Modelos.Utilidades;

public class ConversorMasa : IConversor<UnidadMasa>
{
    public double Convertir(double valor, UnidadMasa unidadOrigen, UnidadMasa unidadFinal)
    {
        var valorEnGramos = ConvertirAGramos(valor, unidadOrigen);
        return ConvertirDesdeGramos(valorEnGramos, unidadFinal);
    }

    private static double ConvertirAGramos(double valor, UnidadMasa unidadOrigen)
    {
        return unidadOrigen switch
        {
            UnidadMasa.Miligramo => valor / 1000.0,
            UnidadMasa.Gramo => valor,
            UnidadMasa.Kilogramo => valor * 1000.0,
            UnidadMasa.Tonelada => valor * 1_000_000.0,
            UnidadMasa.Onza => valor * 28.3495,
            _ => throw new ArgumentException("Unidad no soportada")
        };
    }

    private static double ConvertirDesdeGramos(double valor, UnidadMasa unidadFinal)
    {
        return unidadFinal switch
        {
            UnidadMasa.Miligramo => valor * 1000.0,
            UnidadMasa.Gramo => valor,
            UnidadMasa.Kilogramo => valor / 1000.0,
            UnidadMasa.Tonelada => valor / 1_000_000.0,
            UnidadMasa.Onza => valor / 28.3495,
            _ => throw new ArgumentException("Unidad no soportada")
        };
    }
}
