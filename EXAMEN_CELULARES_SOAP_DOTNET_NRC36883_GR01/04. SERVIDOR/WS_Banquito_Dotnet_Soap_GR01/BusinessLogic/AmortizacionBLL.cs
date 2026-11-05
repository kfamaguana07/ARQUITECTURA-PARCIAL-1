using System;
using System.Collections.Generic;
using WS_Banquito_Dotnet_Soap_GR01.Models;

namespace WS_Banquito_Dotnet_Soap_GR01.BusinessLogic
{
    public class AmortizacionBLL
    {
        private const decimal TasaAnual = 16.5m; // 16.5% anual

        public List<CuotaAmortizacion> GenerarTabla(decimal monto, int plazoMeses)
        {
            if (plazoMeses < 3 || plazoMeses > 18)
            {
                throw new ArgumentException("El plazo del crédito debe estar entre 3 y 18 meses.");
            }

            var tabla = new List<CuotaAmortizacion>();

            // TasaPeriodo = 16.5% / 12 = 0.01375 mensual
            double tasaPeriodo = (double)(TasaAnual / 100m / 12m);

            // Cálculo de Cuota Fija aplicando la fórmula
            // Cuota = Valor / ( (1 - (1+TasaPeriodo)^-NúmeroCuotas) / TasaPeriodo )
            double denominador = (1.0 - Math.Pow(1.0 + tasaPeriodo, -plazoMeses)) / tasaPeriodo;
            decimal valorCuota = Math.Round((decimal)((double)monto / denominador), 2);

            decimal saldo = monto;

            for (int i = 1; i <= plazoMeses; i++)
            {
                decimal interesPagado = Math.Round(saldo * (decimal)tasaPeriodo, 2);
                decimal capitalPagado = valorCuota - interesPagado;

                // Ajuste de precisión para la última cuota
                if (i == plazoMeses)
                {
                    capitalPagado = saldo;
                    valorCuota = capitalPagado + interesPagado;
                    saldo = 0.00m;
                }
                else
                {
                    saldo -= capitalPagado;
                }

                tabla.Add(new CuotaAmortizacion
                {
                    NumCuota = i,
                    ValorCuota = valorCuota,
                    InteresPagado = interesPagado,
                    CapitalPagado = capitalPagado,
                    SaldoRestante = saldo
                });
            }

            return tabla;
        }
    }
}