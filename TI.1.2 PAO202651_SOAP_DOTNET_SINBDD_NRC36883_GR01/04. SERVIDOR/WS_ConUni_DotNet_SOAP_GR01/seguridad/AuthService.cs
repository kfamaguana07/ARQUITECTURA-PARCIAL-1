using System;

namespace ec.edu.monster.Seguridad
{
    // Servicio de dominio para autenticacion y ciclo de vida de credenciales.
    public class AuthService
    {
        private const string UsuarioPermitido = "Monster";
        private readonly object sync = new object();
        private string contraseniaActual = "Monster9";

        public string Autenticar(string usuario, string contrasenia)
        {
            if (!string.Equals(usuario, UsuarioPermitido, StringComparison.Ordinal) ||
                !string.Equals(contrasenia, contraseniaActual, StringComparison.Ordinal))
            {
                return null;
            }

            return AdministradorTokens.GenerarToken();
        }

        public bool CambiarContrasenia(string contraseniaAnterior, string nuevaContrasenia)
        {
            if (string.IsNullOrWhiteSpace(nuevaContrasenia))
            {
                return false;
            }

            lock (sync)
            {
                if (!string.Equals(contraseniaActual, contraseniaAnterior, StringComparison.Ordinal))
                {
                    return false;
                }

                contraseniaActual = nuevaContrasenia;
                AdministradorTokens.LimpiarTokens();
                return true;
            }
        }
    }
}
