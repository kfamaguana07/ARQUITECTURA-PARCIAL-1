package ec.edu.monster.controller.ws;

import ec.edu.monster.controller.ws.dto.CambiarContrasenaRequest;
import ec.edu.monster.controller.ws.dto.LoginRequest;
import ec.edu.monster.controller.ws.dto.LoginResponse;
import ec.edu.monster.controller.ws.dto.MensajeResponse;
import ec.edu.monster.seguridad.AdministradorCredenciales;
import ec.edu.monster.seguridad.AdministradorTokens;
import jakarta.ws.rs.Consumes;
import jakarta.ws.rs.POST;
import jakarta.ws.rs.Path;
import jakarta.ws.rs.Produces;
import jakarta.ws.rs.core.MediaType;
import jakarta.ws.rs.core.Response;

@Path("/seguridad")
@Consumes(MediaType.APPLICATION_JSON)
@Produces(MediaType.APPLICATION_JSON)
public class WSSeguridad {

    @POST
    @Path("/login")
    public Response login(LoginRequest request) {
        if (request == null
                || request.getUsuario() == null
                || request.getUsuario().isBlank()
                || request.getContrasenia() == null
                || request.getContrasenia().isBlank()) {
            return respuestaError(Response.Status.BAD_REQUEST, "Usuario y contrasenia son obligatorios");
        }

        if (AdministradorCredenciales.validarCredenciales(request.getUsuario(), request.getContrasenia())) {
            String token = AdministradorTokens.generarToken();
            return Response.ok(new LoginResponse(token)).build();
        }

        return respuestaError(Response.Status.UNAUTHORIZED, "Credenciales incorrectas");
    }

    @POST
    @Path("/cambiar-contrasena")
    public Response cambiarContrasena(CambiarContrasenaRequest request) {
        if (request == null
                || request.getContraseniaActual() == null
                || request.getContraseniaActual().isBlank()
                || request.getNuevaContrasenia() == null
                || request.getNuevaContrasenia().isBlank()) {
            return respuestaError(Response.Status.BAD_REQUEST, "Contrasenias son obligatorias");
        }

        try {
            AdministradorCredenciales.cambiarContrasenia(
                    request.getContraseniaActual(),
                    request.getNuevaContrasenia());
            return Response.ok(new MensajeResponse("Contrasenia actualizada correctamente")).build();
        } catch (RuntimeException e) {
            return respuestaError(Response.Status.BAD_REQUEST, e.getMessage());
        }
    }

    private static Response respuestaError(Response.Status estado, String mensaje) {
        return Response.status(estado)
                .entity(new MensajeResponse(mensaje))
                .type(MediaType.APPLICATION_JSON)
                .build();
    }
}
