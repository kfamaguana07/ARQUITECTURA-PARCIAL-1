package ec.edu.monster.controller.manejadoresws;

import ec.edu.monster.seguridad.AdministradorTokens;
import jakarta.annotation.Priority;
import jakarta.ws.rs.Priorities;
import jakarta.ws.rs.container.ContainerRequestContext;
import jakarta.ws.rs.container.ContainerRequestFilter;
import jakarta.ws.rs.core.HttpHeaders;
import jakarta.ws.rs.core.MediaType;
import jakarta.ws.rs.core.Response;
import jakarta.ws.rs.ext.Provider;

@Provider
@Priority(Priorities.AUTHENTICATION)
public class ManejadorAutenticacion implements ContainerRequestFilter {

    @Override
    public void filter(ContainerRequestContext contexto) {
        String ruta = contexto.getUriInfo().getPath();
        if (ruta == null) {
            ruta = "";
        }

        if ("OPTIONS".equalsIgnoreCase(contexto.getMethod()) || ruta.endsWith("/login")) {
            return;
        }

        String cabeceraAutorizacion = contexto.getHeaderString(HttpHeaders.AUTHORIZATION);
        if (cabeceraAutorizacion == null || cabeceraAutorizacion.isBlank()) {
            rechazar(contexto, "Acceso denegado: Se requiere el encabezado Authorization con el token.");
            return;
        }

        String token = extraerToken(cabeceraAutorizacion);
        if (token.isBlank() || !AdministradorTokens.validarToken(token)) {
            rechazar(contexto, "Acceso denegado: Token invalido o expirado.");
        }
    }

    private static String extraerToken(String cabeceraAutorizacion) {
        String prefijo = "Bearer ";
        if (cabeceraAutorizacion.regionMatches(true, 0, prefijo, 0, prefijo.length())) {
            return cabeceraAutorizacion.substring(prefijo.length()).trim();
        }
        return cabeceraAutorizacion.trim();
    }

    private static void rechazar(ContainerRequestContext contexto, String mensaje) {
        String json = "{\"mensaje\":\"" + escaparJson(mensaje) + "\"}";
        Response respuesta = Response.status(Response.Status.UNAUTHORIZED)
                .type(MediaType.APPLICATION_JSON)
                .entity(json)
                .build();
        contexto.abortWith(respuesta);
    }

    private static String escaparJson(String texto) {
        if (texto == null) {
            return "";
        }
        return texto.replace("\\", "\\\\").replace("\"", "\\\"");
    }
}