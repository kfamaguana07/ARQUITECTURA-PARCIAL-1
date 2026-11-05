namespace WS_ConUni_DotNet_Rest_GR5.Modelos.Utilidades;

public interface IConversor<TUnidad>
{
    double Convertir(double valor, TUnidad unidadOrigen, TUnidad unidadFinal);
}
