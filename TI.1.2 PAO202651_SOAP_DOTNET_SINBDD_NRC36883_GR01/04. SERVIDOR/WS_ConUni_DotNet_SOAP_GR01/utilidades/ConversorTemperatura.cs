using System;
using ec.edu.monster.Utilidades.Enums;

namespace ec.edu.monster.Utilidades
{
    public class ConversorTemperatura : IConversor<UnidadTemperatura>
    {
        public double Convertir(double valor, UnidadTemperatura origen, UnidadTemperatura destino)
        {
            if (origen == destino)
            {
                return valor;
            }

            switch (origen)
            {
                case UnidadTemperatura.CELSIUS:
                    switch (destino)
                    {
                        case UnidadTemperatura.FAHRENHEIT:
                            return (valor * 9.0 / 5.0) + 32.0;
                        case UnidadTemperatura.KELVIN:
                            return valor + 273.15;
                        case UnidadTemperatura.RANKINE:
                            return (valor + 273.15) * 9.0 / 5.0;
                        default:
                            throw new ArgumentException("Conversion no soportada");
                    }

                case UnidadTemperatura.FAHRENHEIT:
                    switch (destino)
                    {
                        case UnidadTemperatura.CELSIUS:
                            return (valor - 32.0) * 5.0 / 9.0;
                        case UnidadTemperatura.KELVIN:
                            return (valor - 32.0) * 5.0 / 9.0 + 273.15;
                        case UnidadTemperatura.RANKINE:
                            return valor + 459.67;
                        default:
                            throw new ArgumentException("Conversion no soportada");
                    }

                case UnidadTemperatura.KELVIN:
                    switch (destino)
                    {
                        case UnidadTemperatura.CELSIUS:
                            return valor - 273.15;
                        case UnidadTemperatura.FAHRENHEIT:
                            return (valor - 273.15) * 9.0 / 5.0 + 32.0;
                        case UnidadTemperatura.RANKINE:
                            return valor * 9.0 / 5.0;
                        default:
                            throw new ArgumentException("Conversion no soportada");
                    }

                case UnidadTemperatura.RANKINE:
                    switch (destino)
                    {
                        case UnidadTemperatura.CELSIUS:
                            return (valor - 491.67) * 5.0 / 9.0;
                        case UnidadTemperatura.FAHRENHEIT:
                            return valor - 459.67;
                        case UnidadTemperatura.KELVIN:
                            return valor * 5.0 / 9.0;
                        default:
                            throw new ArgumentException("Conversion no soportada");
                    }

                default:
                    throw new ArgumentException("Conversion no soportada");
            }
        }
    }
}

