package ec.edu.monster.controladores;

import ec.edu.monster.vista.ConsolaView;
import ec.edu.monster.ws.WSEurekaBank;
import ec.edu.monster.ws.WSEurekaBank_Service;

/**
 *
 * @author Daniel Guaman <adguaman4@espe.edu.ec>
 */
public class BancoController {
    private final ConsolaView vista;
    private final WSEurekaBank port;

    public BancoController(ConsolaView vista) {
        this.vista = vista;
        WSEurekaBank_Service service = new WSEurekaBank_Service();
        this.port = service.getWSEurekaBankPort();
    }

    public void iniciar() {
        vista.mostrarBienvenida();
        
        boolean autenticado = false;
        while (!autenticado) {
            String[] credenciales = vista.pedirCredenciales();
            try {
                if (port.iniciarSesion(credenciales[0], credenciales[1])) {
                    vista.mostrarMensaje("\n[+] Login exitoso. Bienvenido, " + credenciales[0] + ".");
                    autenticado = true;
                } else {
                    vista.mostrarMensaje("[-] Credenciales incorrectas. Intente de nuevo.\n");
                }
            } catch (Exception e) {
                vista.mostrarMensaje("[-] Error al conectar con el servidor: " + e.getMessage());
                return;
            }
        }
        
        int opcion = 0;
        while (opcion != 4) { // Cambiamos a 4 porque ahora hay 4 opciones
            opcion = vista.mostrarMenuPrincipal();
            switch (opcion) {
                case 1 -> {
                    try {
                        vista.mostrarCuentas(port.traerCuentasConClientes());
                    } catch (Exception e) {
                        vista.mostrarMensaje("[-] Error al obtener cuentas: " + e.getMessage());
                    }
                }
                case 2 -> {
                    String cuenta = vista.pedirCuenta();
                    try {
                        vista.mostrarMovimientos(port.traerMovimientos(cuenta));
                    } catch (Exception e) {
                        vista.mostrarMensaje("[-] Error al obtener movimientos: " + e.getMessage());
                    }
                }
                case 3 -> gestionarOperaciones();
                case 4 -> vista.mostrarMensaje("\nCerrando sesión... ¡Hasta luego!");
                default -> vista.mostrarMensaje("[-] Opción no válida.");
            }
        }
        vista.cerrar();
    }
    
        private void gestionarOperaciones() {
        int opOp = 0;
        while (opOp != 4) {
            opOp = vista.mostrarMenuOperaciones();
            if (opOp == 4) break;

            String tipo = "";
            switch (opOp) {
                case 1 -> tipo = "DEPOSITO";
                case 2 -> tipo = "RETIRO";
                case 3 -> tipo = "TRANSFERENCIA";
                default -> {
                    vista.mostrarMensaje("[-] Opción no válida.");
                    continue;
                }
            }

            String cuentaOrigen = vista.pedirCuenta();
            String cuentaDestino = "";
            
            if (tipo.equals("TRANSFERENCIA")) {
                cuentaDestino = vista.pedirCuentaDestino();
            }

            double importe = vista.pedirImporte();
            if (importe <= 0) {
                vista.mostrarMensaje("[-] El importe debe ser mayor a 0.");
                continue;
            }

            try {
                var resultado = port.regMovimiento(cuentaOrigen, cuentaDestino, tipo, importe);
                vista.mostrarResultadoOperacion(true, resultado.toString());
                
            } catch (Exception e) {
                vista.mostrarResultadoOperacion(false, e.getMessage());
            }
        }
    }
}
