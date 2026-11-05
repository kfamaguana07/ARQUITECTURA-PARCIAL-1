package ec.edu.monster.ws;

import ec.edu.monster.model.DatosCuenta;
import ec.edu.monster.model.MovimientoData;
import ec.edu.monster.model.ResultadoOperacion;
//import ec.edu.monster.model.MovimientoRequestDto;
import ec.edu.monster.service.EurekaService;
import ec.edu.monster.service.Login;
import jakarta.jws.WebService;
import jakarta.jws.WebMethod;
import jakarta.jws.WebParam;
import jakarta.jws.WebResult;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

@WebService(serviceName = "WSEurekaBank")
public class WSEurekaBank {

    private static final String DEFAULT_COD_EMPLEADO = "0001";
    private final EurekaService eurekaService = new EurekaService();
    private final Login loginService = new Login();

    /**
     * 1. Traer movimientos de una cuenta
     */
    @WebMethod(operationName = "traerMovimientos")
    @WebResult(name = "MovimientoData")
    public List<MovimientoData> traerMovimientos(@WebParam(name = "cuenta") String cuenta) {
        try {
            return eurekaService.leerMovimientos(cuenta);
        } catch (Exception e) {
            System.err.println("Error en traerMovimientos: " + e.getMessage());
            return new ArrayList<>();
        }
    }

    /**
     * 2. Registrar movimiento: Depósito, Retiro o Transferencia
     * @param cuentaOrigen
     * @param cuentaDestino
     * @param tipo
     * @param importe
     * @return 
     */
    @WebMethod(operationName = "regMovimiento")
    @WebResult(name = "resultado")
    public ResultadoOperacion regMovimiento(
            @WebParam(name = "cuentaOrigen") String cuentaOrigen,
            @WebParam(name = "cuentaDestino") String cuentaDestino,
            @WebParam(name = "tipo") String tipo,
            @WebParam(name = "importe") double importe) {

        try {
            // Validaciones básicas
            if (cuentaOrigen == null || cuentaOrigen.trim().isEmpty()) {
                return ResultadoOperacion.error("Cuenta origen es requerida");
            }
            if (importe <= 0) {
                return ResultadoOperacion.error("El importe debe ser mayor a 0");
            }
            if (tipo == null || tipo.trim().isEmpty()) {
                return ResultadoOperacion.error("Tipo de movimiento es requerido");
            }

            String tipoUpper = tipo.toUpperCase();

            switch (tipoUpper) {
                case "DEPOSITO" -> {
                    if (cuentaDestino != null && !cuentaDestino.trim().isEmpty()) {
                        return ResultadoOperacion.error("Depósito no debe tener cuenta destino");
                    }
                    eurekaService.registrarDeposito(cuentaOrigen, importe, DEFAULT_COD_EMPLEADO);
                    return ResultadoOperacion.exito();
                }

                case "RETIRO" -> {
                    if (cuentaDestino != null && !cuentaDestino.trim().isEmpty()) {
                        return ResultadoOperacion.error("Retiro no debe tener cuenta destino");
                    }
                    eurekaService.registrarRetiro(cuentaOrigen, importe, DEFAULT_COD_EMPLEADO);
                    return ResultadoOperacion.exito();
                }

                case "TRANSFERENCIA" -> {
                    if (cuentaDestino == null || cuentaDestino.trim().isEmpty()) {
                        return ResultadoOperacion.error("Cuenta destino es requerida para transferencia");
                    }
                    eurekaService.registrarTransferencia(cuentaOrigen, cuentaDestino, importe, DEFAULT_COD_EMPLEADO);
                    return ResultadoOperacion.exito();
                }

                default -> {
                    return ResultadoOperacion.error("Tipo de movimiento no válido: " + tipo);
                }
            }

        } catch (RuntimeException e) {
            // Errores del servicio (saldo insuficiente, cuenta no existe, etc.)
            return ResultadoOperacion.error(e.getMessage());
        } catch (Exception e) {
            System.err.println("Error inesperado: " + e.getMessage());
            e.printStackTrace();
            return ResultadoOperacion.error("Error interno del servidor");
        }
    }

    /**
     * 3. Login de usuario
     * @param usuario
     * @param contrasena
     * @return 
     */
    @WebMethod(operationName = "iniciarSesion")
    public boolean iniciarSesion(
            @WebParam(name = "usuario") String usuario,
            @WebParam(name = "contrasena") String contrasena) {
        try {
            return loginService.IniciarSesion(usuario, contrasena);
        } catch (Exception e) {
            System.err.println("Error en login: " + e.getMessage());
            return false;
        }
    }

    @WebMethod(operationName = "traerCuentasConClientes")
    @WebResult(name = "GetDatosCuentasResult")
    public List<DatosCuenta> traerCuentasConClientes() {
        try {
            return eurekaService.leerCuentasConClientes();
        } catch (Exception e) {
            System.err.println("Error en traerCuentasConClientes: " + e.getMessage());
            return new ArrayList<>();
        }
    }

    // Lista estática inicializada con los tipos permitidos
    private static final List<String> TIPO_MOVIMIENTOS_PERMITIDOS = Arrays.asList(
            "DEPOSITO",
            "RETIRO",
            "TRANSFERENCIA"
    );

    @WebMethod(operationName = "tipoMovimientosPermitidos")
    @WebResult(name = "tipoMovimiento")
    public List<String> obtenerTipoMovimientosPermitidos() {
        try {
            return new ArrayList<>(TIPO_MOVIMIENTOS_PERMITIDOS); // Devuelve una copia
        } catch (Exception e) {
            System.err.println("Error al obtener los tipos de movimientos: " + e.getMessage());
            return new ArrayList<>();
        }
    }
}
