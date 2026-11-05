using System;
using ec.edu.monster.Utilidades.Enums;

namespace ec.edu.monster.Mapeadores
{
    public static class UnidadMapper
    {
        public static UnidadLongitud ToLongitud(string unidad)
        {
            return ParseEnum<UnidadLongitud>(unidad, "Unidad de longitud invalida: ");
        }

        public static UnidadMasa ToMasa(string unidad)
        {
            return ParseEnum<UnidadMasa>(unidad, "Unidad de masa invalida: ");
        }

        public static UnidadTemperatura ToTemperatura(string unidad)
        {
            return ParseEnum<UnidadTemperatura>(unidad, "Unidad de temperatura invalida: ");
        }

        private static T ParseEnum<T>(string unidad, string prefijoError) where T : struct
        {
            var normalizada = (unidad ?? string.Empty).Trim().ToUpperInvariant();
            T valor;

            if (!Enum.TryParse(normalizada, out valor) || !Enum.IsDefined(typeof(T), valor))
            {
                throw new ArgumentException(prefijoError + unidad);
            }

            return valor;
        }
    }
}

