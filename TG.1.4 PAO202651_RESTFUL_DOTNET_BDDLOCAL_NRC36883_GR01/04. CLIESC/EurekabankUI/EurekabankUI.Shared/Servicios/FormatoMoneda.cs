using System.Globalization;

namespace ec.edu.monster.Servicios;

public static class FormatoMoneda
{
    public static string Mostrar(decimal monto) =>
        "$" + monto.ToString("N2", CultureInfo.InvariantCulture);
}
