using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WS_ConUni_DotNet_Rest_GR5.Seguridad;

namespace WS_ConUni_DotNet_Rest_GR5.Controladores;

[ApiController]
[Route("api/seguridad")]
public class SeguridadController : ControllerBase
{
    private readonly AuthService _authService;

    public SeguridadController(AuthService authService)
    {
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public ActionResult<LoginResponse> Login([FromBody] LoginRequest request)
    {
        var token = _authService.Autenticar(request.Usuario, request.Contrasena);
        if (token is null)
        {
            return Unauthorized("Credenciales inválidas");
        }

        return Ok(new LoginResponse { Token = token });
    }

    [Authorize(AuthenticationSchemes = "BearerToken")]
    [HttpPost("cambiar-contrasena")]
    public IActionResult CambiarContrasena([FromBody] CambiarContrasenaRequest request)
    {
        var cambioExitoso = _authService.CambiarContrasena(request.ContrasenaActual, request.NuevaContrasena);
        if (!cambioExitoso)
        {
            return BadRequest("No se pudo cambiar la contraseña");
        }

        return Ok("Contraseña actualizada");
    }
}
