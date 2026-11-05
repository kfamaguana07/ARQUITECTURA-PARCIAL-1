using System;
using System.Collections.Concurrent;

namespace ec.edu.monster.Seguridad
{
    public static class AdministradorTokens
    {
        private static readonly ConcurrentDictionary<string, DateTime> TokensActivos =
            new ConcurrentDictionary<string, DateTime>();

        private static readonly TimeSpan TiempoVigenciaToken = TimeSpan.FromHours(8);

        public static string GenerarToken()
        {
            var token = Guid.NewGuid().ToString();
            TokensActivos[token] = DateTime.UtcNow.Add(TiempoVigenciaToken);
            return token;
        }

        public static bool ValidarToken(string token)
        {

            if (string.IsNullOrWhiteSpace(token))
                return false;

            string tokenLimpio = token.Trim();
            DateTime expiraEn;
            if (!TokensActivos.TryGetValue(tokenLimpio, out expiraEn))
            {
                return false;
            }

            if (DateTime.UtcNow <= expiraEn)
            {
                return true;
            }

            DateTime valorDescartado;
            TokensActivos.TryRemove(tokenLimpio, out valorDescartado);
            return false;
        }

        public static void LimpiarTokens()
        {
            TokensActivos.Clear();
        }

        public static int ContarTokensEnMemoria()
        {
            return TokensActivos.Count;
        }
    }
}

