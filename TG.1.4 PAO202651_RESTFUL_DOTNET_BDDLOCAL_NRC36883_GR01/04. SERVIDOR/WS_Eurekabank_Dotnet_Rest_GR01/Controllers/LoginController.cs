using Microsoft.AspNetCore.Mvc;
using WS_Eurekabank_Dotnet_Rest_GR01.DTOs;
using WS_Eurekabank_Dotnet_Rest_GR01.Models;
using System.Security.Cryptography;
using System.Text;

namespace WS_Eurekabank_Dotnet_Rest_GR01.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Ruta: /api/login
    public class LoginController : ControllerBase
    {
        private readonly EurekaBankContext _context;

        public LoginController(EurekaBankContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult IniciarSesion([FromBody] LoginRequestDTO request)
        {
            if (request == null || string.IsNullOrEmpty(request.Usuario) || string.IsNullOrEmpty(request.Contrasena))
            {
                return BadRequest(ResultadoOperacionDTO.Error("Usuario y contraseña son requeridos"));
            }

            // Calculamos el hash SHA-1 de la contraseña
            string hashClave = CalcularHashSHA1(request.Contrasena);

            // Buscamos en la base de datos
            var usuario = _context.Usuarios.FirstOrDefault(u => 
                u.VchEmplUsuario == request.Usuario && 
                u.VchEmplClave == hashClave && 
                u.VchEmplEstado == "ACTIVO");

            if (usuario != null)
            {
                return Ok(ResultadoOperacionDTO.Exito("Login exitoso"));
            }

            return Unauthorized(ResultadoOperacionDTO.Error("Credenciales incorrectas"));
        }

        private string CalcularHashSHA1(string input)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] hashBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}