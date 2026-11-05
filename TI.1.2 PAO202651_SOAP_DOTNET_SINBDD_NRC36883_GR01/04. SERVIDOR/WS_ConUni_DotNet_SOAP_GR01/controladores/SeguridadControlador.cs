using ec.edu.monster.Seguridad;

namespace ec.edu.monster.Controladores
{
    // Controlador de autenticacion para separar responsabilidades de la capa de servicio WCF.
    public class SeguridadControlador
    {
        private static readonly AuthService authService = new AuthService();

        public string Login(string usuario, string contrasenia)
        {
            return authService.Autenticar(usuario, contrasenia);
        }

        public bool CambiarContrasenia(string contraseniaActual, string nuevaContrasenia)
        {
            return authService.CambiarContrasenia(contraseniaActual, nuevaContrasenia);
        }
    }
}
