namespace ec.edu.monster.Utilidades
{
    public interface IConversor<T>
    {
        double Convertir(double valor, T unidadInicial, T unidadFinal);
    }
}

