using ComercializadoraServiceReference;

namespace Comercializadora_ClienteConsola_Dotnet_GR01.Views
{
    public class AmortizacionView
    {
        public int CapturarCodigoCredito()
        {
            ConsoleHelper.MostrarTitulo("CONSULTAR TABLA DE AMORTIZACIÓN BANCARIA");
            return ConsoleHelper.LeerEntero("Ingrese el Código de Crédito Otorgado", 1);
        }

        public void MostrarTablaAmortizacion(IEnumerable<CuotaAmortizacion> tabla, int codigoCredito)
        {
            ConsoleHelper.MostrarTitulo($"TABLA DE AMORTIZACIÓN (SISTEMA FRANCÉS - 16.5% TASA ANUAL) - CRÉDITO #{codigoCredito}");

            Console.WriteLine(string.Format("{0,-8} | {1,-14} | {2,-14} | {3,-14} | {4,-14}",
                "CUOTA #", "VALOR CUOTA", "INTERÉS", "CAPITAL", "SALDO RESTANTE"));
            Console.WriteLine(new string('-', 75));

            int totalCuotas = 0;
            decimal totalInteres = 0;
            decimal totalCapital = 0;

            foreach (var c in tabla)
            {
                Console.WriteLine(string.Format("{0,-8} | ${1,-13:N2} | ${2,-13:N2} | ${3,-13:N2} | ${4,-13:N2}",
                    c.NumCuota, c.ValorCuota, c.InteresPagado, c.CapitalPagado, c.SaldoRestante));

                totalCuotas++;
                totalInteres += c.InteresPagado;
                totalCapital += c.CapitalPagado;
            }

            if (totalCuotas == 0)
            {
                ConsoleHelper.MostrarMensajeError($"No se encontraron registros de amortización para el crédito #{codigoCredito}.");
            }
            else
            {
                Console.WriteLine(new string('-', 75));
                Console.WriteLine(string.Format("{0,-8} | ${1,-13:N2} | ${2,-13:N2} | ${3,-13:N2} |",
                    "TOTALES", (totalInteres + totalCapital), totalInteres, totalCapital));
                Console.WriteLine(new string('-', 75));
            }
        }
    }
}