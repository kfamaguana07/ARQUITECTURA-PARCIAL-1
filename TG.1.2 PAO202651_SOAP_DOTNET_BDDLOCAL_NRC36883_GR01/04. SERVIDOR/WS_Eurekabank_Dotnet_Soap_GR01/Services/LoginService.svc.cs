using System.Linq;
using System.Security.Cryptography;
using System.Text;
using WS_Eurekabank_Dotnet_Soap_GR01.Interfaces;
using WS_Eurekabank_Dotnet_Soap_GR01.Models;

namespace WS_Eurekabank_Dotnet_Soap_GR01.Services
{
    public class LoginService : ILoginService
    {
        public bool Login(string usuario, string clave)
        {
            // 1. Calculamos el hash SHA-1 de la contraseña que ingresa el usuario
            string hashClave = CalcularHashSHA1(clave);

            using (var context = new EurekaBankContext())
            {
                // 2. Comparamos el usuario y el hash calculado contra la base de datos
                return context.Usuarios.Any(u =>
                    u.VchEmplUsuario == usuario &&
                    u.VchEmplClave == hashClave &&
                    u.VchEmplEstado == "ACTIVO");
            }
        }

        // Método auxiliar para calcular el hash
        private string CalcularHashSHA1(string input)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] hashBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("X2")); // "X2" convierte a mayúsculas
                }
                return sb.ToString();
            }
        }
    }
}