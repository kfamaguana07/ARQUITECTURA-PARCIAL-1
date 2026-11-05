namespace WS_ConUni_DotNet_Rest_GR5.Modelos;

public class ConversionRequest
{
    public double Valor { get; set; }
    public string UnidadOrigen { get; set; } = string.Empty;
    public string UnidadDestino { get; set; } = string.Empty;
}
