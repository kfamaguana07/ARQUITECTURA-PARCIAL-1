package ec.edu.monster.servicio;

import ec.edu.monster.dao.CuentaDAO;
import ec.edu.monster.dao.LoginDAO;
import ec.edu.monster.dao.MovimientoDAO;
import ec.edu.monster.model.DatosCuenta;
import ec.edu.monster.model.MovimientoData;
import ec.edu.monster.model.MovimientoRequestDto;
import ec.edu.monster.model.ResultadoOperacion;

import jakarta.enterprise.context.RequestScoped;
import jakarta.ws.rs.*;
import jakarta.ws.rs.core.*;
import java.util.Arrays;
import java.util.List;

@Path("eurekabank")
@RequestScoped
@Produces(MediaType.APPLICATION_JSON)
@Consumes(MediaType.APPLICATION_JSON)
public class EurekaBank {

    private static final String DEFAULT_COD_EMPLEADO = "0001";
    
    private final CuentaDAO cuentaDAO = new CuentaDAO();
    private final MovimientoDAO movimientoDAO = new MovimientoDAO();
    private final LoginDAO loginDAO = new LoginDAO();

    @GET
    @Path("movimientos")
    public Response traerMovimientos(@QueryParam("cuenta") String cuenta) {
        try {
            List<MovimientoData> lista = movimientoDAO.leerMovimientos(cuenta);
            return Response.ok(lista).build();
        } catch (Exception e) {
            return Response.status(500)
                    .entity(ResultadoOperacion.error("Error al obtener movimientos: " + e.getMessage()))
                    .build();
        }
    }

    @POST
    @Path("movimientos")
    public Response registrarMovimiento(MovimientoRequestDto req) {
        try {
            if (req.getCuentaOrigen() == null || req.getCuentaOrigen().trim().isEmpty()) {
                return Response.status(400).entity(ResultadoOperacion.error("Cuenta origen requerida")).build();
            }
            if (req.getImporte() <= 0) {
                return Response.status(400).entity(ResultadoOperacion.error("Importe debe ser > 0")).build();
            }
            if (req.getTipo() == null || req.getTipo().trim().isEmpty()) {
                return Response.status(400).entity(ResultadoOperacion.error("Tipo requerido")).build();
            }

            String tipoUpper = req.getTipo().toUpperCase();

            switch (tipoUpper) {
                case "DEPOSITO":
                    if (req.getCuentaDestino() != null && !req.getCuentaDestino().trim().isEmpty()) {
                        return Response.status(400).entity(ResultadoOperacion.error("Depósito no debe tener cuenta destino")).build();
                    }
                    movimientoDAO.realizarMovimiento(req.getCuentaOrigen(), req.getImporte(), DEFAULT_COD_EMPLEADO, "DEPOSITO", null);
                    break;

                case "RETIRO":
                    if (req.getCuentaDestino() != null && !req.getCuentaDestino().trim().isEmpty()) {
                        return Response.status(400).entity(ResultadoOperacion.error("Retiro no debe tener cuenta destino")).build();
                    }
                    movimientoDAO.realizarMovimiento(req.getCuentaOrigen(), req.getImporte(), DEFAULT_COD_EMPLEADO, "RETIRO", null);
                    break;

                case "TRANSFERENCIA":
                    if (req.getCuentaDestino() == null || req.getCuentaDestino().trim().isEmpty()) {
                        return Response.status(400).entity(ResultadoOperacion.error("Cuenta destino requerida")).build();
                    }
                    movimientoDAO.realizarMovimiento(req.getCuentaOrigen(), req.getImporte(), DEFAULT_COD_EMPLEADO, "TRANSFERENCIA", req.getCuentaDestino());
                    break;

                default:
                    return Response.status(400).entity(ResultadoOperacion.error("Tipo inválido: " + req.getTipo())).build();
            }

            return Response.ok(ResultadoOperacion.exito()).build();

        } catch (RuntimeException e) {
            return Response.status(400).entity(ResultadoOperacion.error(e.getMessage())).build();
        } catch (Exception e) {
            return Response.status(500).entity(ResultadoOperacion.error("Error interno del servidor")).build();
        }
    }

    public static class LoginRequest {
        public String usuario;
        public String contrasena;
    }

    @POST
    @Path("login")
    public Response login(LoginRequest req) {
        try {
            if (req.usuario == null || req.usuario.trim().isEmpty()
                    || req.contrasena == null || req.contrasena.trim().isEmpty()) {
                return Response.status(400).entity(ResultadoOperacion.error("Usuario y contraseña requeridos")).build();
            }

            boolean ok = loginDAO.iniciarSesion(req.usuario, req.contrasena);
            return Response.ok(
                    new ResultadoOperacion(ok ? 1 : -1, ok ? "Login exitoso" : "Credenciales inválidas")
            ).build();

        } catch (Exception e) {
            return Response.status(500).entity(ResultadoOperacion.error("Error en login")).build();
        }
    }

    @GET
    @Path("cuentas")
    public Response traerCuentasConClientes() {
        try {
            List<DatosCuenta> lista = cuentaDAO.leerCuentasConClientes();
            return Response.ok(lista).build();
        } catch (Exception e) {
            return Response.status(500).entity(ResultadoOperacion.error("Error al obtener cuentas")).build();
        }
    }

    private static final List<String> TIPOS_PERMITIDOS = Arrays.asList("DEPOSITO", "RETIRO", "TRANSFERENCIA");

    @GET
    @Path("tipos")
    public Response obtenerTiposPermitidos() {
        return Response.ok(TIPOS_PERMITIDOS).build();
    }
}