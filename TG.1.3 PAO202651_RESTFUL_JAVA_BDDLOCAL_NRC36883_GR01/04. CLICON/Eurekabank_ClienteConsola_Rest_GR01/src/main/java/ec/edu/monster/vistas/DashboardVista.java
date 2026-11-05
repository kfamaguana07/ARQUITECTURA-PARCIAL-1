package ec.edu.monster.vistas;

import ec.edu.monster.model.DatosCuenta;
import ec.edu.monster.model.MovimientoData;
import ec.edu.monster.model.MovimientoRequestDto;
import ec.edu.monster.model.ResultadoOperacion;
import ec.edu.monster.servicios.EuBankService;

import java.text.DecimalFormat;
import java.text.DecimalFormatSymbols;
import java.util.ArrayList;
import java.util.List;
import java.util.Locale;
import java.util.Scanner;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.time.format.DateTimeParseException;

/**
 *
 * @author Daniel Guaman <adguaman4@espe.edu.ec>
 */
public class DashboardVista {

    private final EuBankService euBankService;
    private final Scanner scanner;
    private final DecimalFormat decimalFormat;

    public DashboardVista(EuBankService euBankService) {
        this.euBankService = euBankService;
        this.scanner = new Scanner(System.in, "UTF-8");
        DecimalFormatSymbols symbols = new DecimalFormatSymbols(Locale.US);
        this.decimalFormat = new DecimalFormat("#,##0.00", symbols);
    }

    public void mostrarMenuPrincipal() {
        while (true) {
            limpiarPantalla();
            System.out.println("\n╔══════════════════════════════════════════╗");
            System.out.println("║      Dashboard - EuBank (REST)      ║");
            System.out.println("╚══════════════════════════════════════════╝");
            System.out.println("\n1. Ver Cuentas de Clientes");
            System.out.println("2. Ver Movimientos");
            System.out.println("3. Registrar Depósito");
            System.out.println("4. Registrar Retiro");
            System.out.println("5. Registrar Transferencia");
            System.out.println("6. Salir");
            System.out.print("\nSeleccione una opción: ");

            int opcion;
            try {
                opcion = Integer.parseInt(scanner.nextLine()); 
            } catch (Exception e) {
                mostrarError("Opción no válida. Debe ser un número.");
                esperarTecla();
                continue;
            }

            switch (opcion) {
                case 1:
                    mostrarCuentasClientes();
                    break;
                case 2:
                    mostrarMovimientos();
                    break;
                case 3:
                    registrarMovimiento("DEPOSITO");
                    break;
                case 4:
                    registrarMovimiento("RETIRO");
                    break;
                case 5:
                    registrarTransferencia();
                    break;
                case 6:
                    System.out.println("\n¡Gracias por usar EuBank! ¡Adiós!");
                    return;
                default:
                    mostrarError("Opción no válida. Seleccione de 1 a 6.");
                    esperarTecla();
                    break;
            }
        }
    }

    private void mostrarCuentasClientes() {
        limpiarPantalla();
        System.out.println("--- Cuentas de Clientes ---");
        try {
            System.out.print("Cargando cuentas...");
            // CORRECCIÓN: el método en EuBankService se llama obtenerCuentas()
            List<DatosCuenta> cuentas = euBankService.obtenerCuentas();
            System.out.println(" ✓");
            
            if (cuentas == null || cuentas.isEmpty()) {
                System.out.println("No se encontraron cuentas.");
            } else {
                String header = String.format("%-10s | %-30.30s | %-10s | %-10s | %12s",
                        "CUENTA", "CLIENTE", "MONEDA", "ESTADO", "SALDO");
                System.out.println(header);
                System.out.println("-".repeat(header.length()));
                for (DatosCuenta c : cuentas) {
                    System.out.printf("%-10s | %-30.30s | %-10s | %-10s | %12s%n",
                            c.getCodigo(),
                            c.getNombreCliente(),
                            c.getMoneda(),
                            c.getEstado(),
                            decimalFormat.format(c.getSaldo()));
                }
            }
        } catch (Exception e) {
            mostrarError("Error al cargar cuentas: " + e.getMessage());
        }
        esperarTecla();
    }

    private void mostrarMovimientos() {
        limpiarPantalla();
        System.out.println("--- Movimientos por Cuenta ---");
        System.out.print("Ingrese el número de cuenta (ej. 00100001) o presione Enter para ver todos: ");
        String cuenta = scanner.nextLine().trim();

        if (cuenta.isEmpty()) {
            mostrarTodosLosMovimientos();
        } else {
            mostrarMovimientosDeUnaCuenta(cuenta);
        }
        esperarTecla();
    }

    private void mostrarTodosLosMovimientos() {
        System.out.print("Cargando todas las cuentas...");
        try {
            List<DatosCuenta> cuentas = euBankService.obtenerCuentas();
            System.out.println(" ✓");
            List<MovimientoData> todosMovimientos = new ArrayList<>();
            
            System.out.print("Cargando todos los movimientos...");
            for (DatosCuenta cuenta : cuentas) {
                try {
                    // CORRECCIÓN: obtenerMovimientos
                    todosMovimientos.addAll(euBankService.obtenerMovimientos(cuenta.getCodigo()));
                } catch (Exception ex) {
                    // Ignora errores si una cuenta no tiene movs
                }
            }
            System.out.println(" ✓");

            if (todosMovimientos.isEmpty()) {
                System.out.println("No se encontraron movimientos en ninguna cuenta.");
                return;
            }
            
            // Ordenamiento (igual que tu código)
            DateTimeFormatter formatter = DateTimeFormatter.ofPattern("dd/MM/yyyy, HH:mm");
            todosMovimientos.sort((m1, m2) -> {
                LocalDateTime fecha1 = null;
                LocalDateTime fecha2 = null;
                try {
                    if (m1.getFecha() != null && !m1.getFecha().isEmpty()) 
                        fecha1 = LocalDateTime.parse(m1.getFecha(), formatter);
                } catch (DateTimeParseException e) {}
                try {
                    if (m2.getFecha() != null && !m2.getFecha().isEmpty()) 
                        fecha2 = LocalDateTime.parse(m2.getFecha(), formatter);
                } catch (DateTimeParseException e) {}

                if (fecha1 != null && fecha2 != null) {
                    int resFecha = fecha2.compareTo(fecha1);
                    if (resFecha != 0) return resFecha;
                    return Integer.compare(m2.getNumero(), m1.getNumero());
                }
                else if (fecha1 == null && fecha2 != null) return 1;
                else if (fecha1 != null && fecha2 == null) return -1;
                else return Integer.compare(m2.getNumero(), m1.getNumero());
            });

            String header = String.format("%-10s | %-4s | %-17s | %-25.25s | %12s | %12s",
                    "CUENTA", "NRO", "FECHA", "TIPO", "IMPORTE", "SALDO");
            System.out.println(header);
            System.out.println("-".repeat(header.length()));
            
            for (MovimientoData m : todosMovimientos) {
                String tipo = m.getTipo() != null ? m.getTipo() : "N/A"; 
                System.out.printf("%-10s | %-4d | %-17s | %-25.25s | %12s | %12s%n",
                        m.getCodigoCuenta(), 
                        m.getNumero(),
                        m.getFecha(), 
                        tipo,
                        decimalFormat.format(m.getImporte()),
                        decimalFormat.format(m.getSaldoActual()));
            }

        } catch (Exception e) {
            System.out.println(" ✗");
            mostrarError("Error al cargar todos los movimientos: " + e.getMessage());
        }
    }
    
    private void mostrarMovimientosDeUnaCuenta(String cuenta) {
        try {
            System.out.print("Cargando movimientos de la cuenta " + cuenta + "...");
            List<MovimientoData> movimientos = euBankService.obtenerMovimientos(cuenta);
            System.out.println(" ✓");
            
            if (movimientos == null || movimientos.isEmpty()) {
                System.out.println("No se encontraron movimientos para la cuenta " + cuenta);
            } else {
                String header = String.format("%-4s | %-17s | %-25.25s | %12s | %12s",
                        "NRO", "FECHA", "TIPO", "IMPORTE", "SALDO");
                System.out.println(header);
                System.out.println("-".repeat(header.length()));
                
                for (MovimientoData m : movimientos) {
                    String tipo = m.getTipo() != null ? m.getTipo() : "N/A"; 
                    System.out.printf("%-4d | %-17s | %-25.25s | %12s | %12s%n",
                            m.getNumero(),
                            m.getFecha(), 
                            tipo,
                            decimalFormat.format(m.getImporte()),
                            decimalFormat.format(m.getSaldoActual()));
                }
            }
        } catch (Exception e) {
            System.out.println(" ✗");
            mostrarError("Error al cargar movimientos: " + e.getMessage());
        }
    }

    private void registrarMovimiento(String tipo) {
        limpiarPantalla();
        System.out.println("--- Registrar " + tipo + " ---");
        
        System.out.print("Número de Cuenta: ");
        String cuenta = scanner.nextLine().trim();
        System.out.print("Importe: ");
        double importe = 0.0;
        try {
            importe = Double.parseDouble(scanner.nextLine().trim().replace(',', '.'));
            if (importe <= 0) {
                 mostrarError("El importe debe ser positivo.");
                 esperarTecla();
                 return;
            }
        } catch (Exception e) {
            mostrarError("Importe no válido.");
            esperarTecla();
            return;
        }

        try {
            System.out.print("Procesando " + tipo + "...");
            
            // CORRECCIÓN: Usamos el DTO de tu paquete de modelos
            MovimientoRequestDto request = new MovimientoRequestDto();
            request.setTipo(tipo);
            request.setCuentaOrigen(cuenta);
            request.setCuentaDestino("");
            request.setImporte(importe);
            
            ResultadoOperacion res = euBankService.registrarMovimiento(request);
            System.out.println(" ✓");
            
            if (res.getCodigo() == 1) { 
                System.out.println("\n¡Operación exitosa!");
                if(res.getMensaje() != null) System.out.println(res.getMensaje());
            } else {
                mostrarError(res.getMensaje());
            }
        } catch (Exception e) {
            System.out.println(" ✗");
            mostrarError("Error al registrar movimiento: " + e.getMessage());
        }
        esperarTecla();
    }
    
    private void registrarTransferencia() {
        limpiarPantalla();
        System.out.println("--- Registrar Transferencia ---");

        System.out.print("Número de Cuenta Origen: ");
        String cuentaOrigen = scanner.nextLine().trim();
        System.out.print("Número de Cuenta Destino: ");
        String cuentaDestino = scanner.nextLine().trim();
        System.out.print("Importe: ");
        double importe = 0.0;
        try {
            importe = Double.parseDouble(scanner.nextLine().trim().replace(',', '.'));
             if (importe <= 0) {
                 mostrarError("El importe debe ser positivo.");
                 esperarTecla();
                 return;
            }
        } catch (Exception e) {
            mostrarError("Importe no válido.");
            esperarTecla();
            return;
        }
        
        if (cuentaOrigen.equals(cuentaDestino)) {
            mostrarError("Las cuentas de origen y destino no pueden ser la misma.");
            esperarTecla();
            return;
        }

        try {
            System.out.print("Procesando Transferencia...");
            
            MovimientoRequestDto request = new MovimientoRequestDto();
            request.setTipo("TRANSFERENCIA");
            request.setCuentaOrigen(cuentaOrigen);
            request.setCuentaDestino(cuentaDestino);
            request.setImporte(importe);
            
            ResultadoOperacion res = euBankService.registrarMovimiento(request);
            System.out.println(" ✓");
            
            if (res.getCodigo() == 1) {
                System.out.println("\n¡Operación exitosa!");
                if(res.getMensaje() != null) System.out.println(res.getMensaje());
            } else {
                mostrarError(res.getMensaje());
            }
        } catch (Exception e) { 
            System.out.println(" ✗");
            mostrarError("Error al registrar transferencia: " + e.getMessage());
        }
        esperarTecla();
    }

    private void mostrarError(String mensaje) {
        System.out.print("\u001B[31m"); 
        System.out.println("\n✗ " + mensaje);
        System.out.print("\u001B[0m"); 
    }

    private void esperarTecla() {
        System.out.println("\nPresione Enter para continuar...");
        try {
            scanner.nextLine();
        } catch (Exception e) {
        }
    }

    private void limpiarPantalla() {
        try {
            if (System.getProperty("os.name").contains("Windows")) {
                new ProcessBuilder("cmd", "/c", "cls").inheritIO().start().waitFor();
            } else {
                System.out.print("\033[H\033[2J");
                System.out.flush();
            }
        } catch (Exception e) {
            System.out.println("\n".repeat(50));
        }
    }
}