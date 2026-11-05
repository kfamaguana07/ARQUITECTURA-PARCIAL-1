using System;
using WS_Banquito_Dotnet_Soap_GR01.DataAccess;

namespace WS_Banquito_Dotnet_Soap_GR01.BusinessLogic
{
    public class CapacidadPagoBLL
    {
        private readonly MovimientoDAL _movimientoDAL = new MovimientoDAL();

        public decimal CalcularMontoMaximo(int codCliente)
        {
            _movimientoDAL.ObtenerPromediosUltimos3Meses(codCliente, out decimal promedioDepositos, out decimal promedioRetiros);

            decimal diferencia = promedioDepositos - promedioRetiros;
            if (diferencia <= 0)
            {
                return 0;
            }

            // Aplicación estricta de la fórmula dada en el examen: ((Promedio Depósitos – Promedio Retiros) * 30%) * 6
            decimal montoMaximo = (diferencia * 0.30m) * 6m;
            return Math.Round(montoMaximo, 2);
        }
    }
}