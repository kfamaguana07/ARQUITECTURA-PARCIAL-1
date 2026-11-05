namespace WS_ConUni_DotNet_Rest_GR5.Modelos;

public class ConversionResponse
{
    public double ValorOriginal { get; set; }
    public string UnidadOrigen { get; set; } = string.Empty;
    public double ValorConvertido { get; set; }
    public string UnidadDestino { get; set; } = string.Empty;
}
