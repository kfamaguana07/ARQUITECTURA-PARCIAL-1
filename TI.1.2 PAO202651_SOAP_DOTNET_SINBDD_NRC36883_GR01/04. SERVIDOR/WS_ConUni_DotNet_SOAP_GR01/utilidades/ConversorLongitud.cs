using System;
using System.Collections.Generic;
using ec.edu.monster.Utilidades.Enums;

namespace ec.edu.monster.Utilidades
{
    public class ConversorLongitud : IConversor<UnidadLongitud>
    {
        private static readonly Dictionary<UnidadLongitud, double> Factores =
            new Dictionary<UnidadLongitud, double>
            {
                { UnidadLongitud.MILIMETRO, 0.001 },
                { UnidadLongitud.CENTIMETRO, 0.01 },
                { UnidadLongitud.METRO, 1.0 },
                { UnidadLongitud.KILOMETRO, 1000.0 },
                { UnidadLongitud.YARDA, 0.9144 }
            };

        public double Convertir(double valor, UnidadLongitud unidadOrigen, UnidadLongitud unidadFinal)
        {
            if (!Factores.ContainsKey(unidadOrigen) || !Factores.ContainsKey(unidadFinal))
            {
                throw new ArgumentException("Unidad no soportada");
            }

            var valorEnBase = valor * Factores[unidadOrigen];
            return valorEnBase / Factores[unidadFinal];
        }
    }
}

