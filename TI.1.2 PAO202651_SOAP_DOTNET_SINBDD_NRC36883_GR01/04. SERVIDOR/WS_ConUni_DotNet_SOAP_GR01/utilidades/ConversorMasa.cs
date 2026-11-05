using System;
using System.Collections.Generic;
using ec.edu.monster.Utilidades.Enums;

namespace ec.edu.monster.Utilidades
{
    public class ConversorMasa : IConversor<UnidadMasa>
    {
        private static readonly Dictionary<UnidadMasa, double> Factores =
            new Dictionary<UnidadMasa, double>
            {
                { UnidadMasa.MILIGRAMO, 0.001 },
                { UnidadMasa.GRAMO, 1.0 },
                { UnidadMasa.KILOGRAMO, 1000.0 },
                { UnidadMasa.TONELADA, 1000000.0 },
                { UnidadMasa.ONZA, 28.3495 }
            };

        public double Convertir(double valor, UnidadMasa unidadOrigen, UnidadMasa unidadFinal)
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

