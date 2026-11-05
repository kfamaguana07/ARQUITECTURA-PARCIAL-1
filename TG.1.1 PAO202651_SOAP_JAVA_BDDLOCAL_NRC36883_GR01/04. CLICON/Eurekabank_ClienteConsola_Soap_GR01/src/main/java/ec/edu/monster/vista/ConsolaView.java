package ec.edu.monster.vista;

import java.util.List;
import java.util.Scanner;
import ec.edu.monster.ws.DatosCuenta;
import ec.edu.monster.ws.MovimientoData;

/**
 *
 * @author Daniel Guaman <adguaman4@espe.edu.ec>
 */
public class ConsolaView {
    private final Scanner scanner;

    public ConsolaView() {
        this.scanner = new Scanner(System.in);
    }

    public void mostrarBienvenida() {
        System.out.println("=========================================");
        System.out.println("     SISTEMA BANCARIO EUREKA BANK        ");
        System.out.println("           Cliente de Consola GR01           ");
        System.out.println("=========================================");
    }

    public String[] pedirCredenciales() {
        System.out.print("Usuario: ");
        String usuario = scanner.nextLine();
        System.out.print("Contrasenia: ");
        String contrasena = scanner.nextLine();
        return new String[]{usuario, contrasena};
    }

    public void mostrarMensaje(String mensaje) {
        System.out.println(mensaje);
    }

    public int mostrarMenuPrincipal() {
        System.out.println("\n--- MENÚ PRINCIPAL ---");
        System.out.println("1. Ver todas las cuentas");
        System.out.println("2. Ver movimientos de una cuenta");
        System.out.println("3. Centro de Operaciones (Depósito/Retiro/Transferencia)"); 
        System.out.println("4. Salir");
        System.out.print("Seleccione una opción: ");
        try {
            return Integer.parseInt(scanner.nextLine());
        } catch (NumberFormatException e) {
            return -1; // Opción inválida
        }
    }

    public String pedirCuenta() {
        System.out.print("\nIngrese el código de la cuenta (ej. 00200001): ");
        return scanner.nextLine();
    }

    public void mostrarCuentas(List<DatosCuenta> cuentas) {
        System.out.println("\n--- LISTA DE CUENTAS ACTIVAS ---");
        if (cuentas == null || cuentas.isEmpty()) {
            System.out.println("No hay cuentas registradas.");
            return;
        }
        System.out.printf("%-10s | %-30s | %-10s | %-10s%n", "CUENTA", "CLIENTE", "MONEDA", "SALDO");
        System.out.println("--------------------------------------------------------------");
        for (DatosCuenta c : cuentas) {
            System.out.printf("%-10s | %-30s | %-10s | %10.2f%n", 
                    c.getCodigo(), c.getNombreCliente(), c.getMoneda(), c.getSaldo().doubleValue());
        }
    }

    public void mostrarMovimientos(List<MovimientoData> movimientos) {
        System.out.println("\n--- MOVIMIENTOS DE LA CUENTA ---");
        if (movimientos == null || movimientos.isEmpty()) {
            System.out.println("No hay movimientos registrados.");
            return;
        }
        System.out.printf("%-5s | %-12s | %-25s | %-10s | %-10s%n", "NRO", "FECHA", "TIPO", "IMPORTE", "SALDO");
        System.out.println("---------------------------------------------------------------------------------");
        for (MovimientoData m : movimientos) {
            String fechaStr = m.getFecha().toString();
            if (fechaStr.length() > 10) fechaStr = fechaStr.substring(0, 10);
            
            System.out.printf("%-5d | %-12s | %-25s | %10.2f | %10.2f%n", 
                    m.getNumero(), fechaStr, m.getTipo(), m.getImporte().doubleValue(), m.getSaldoActual().doubleValue());
        }
    }
    
        // --- MÉTODOS PARA OPERACIONES ---

    public int mostrarMenuOperaciones() {
        System.out.println("\n--- CENTRO DE OPERACIONES ---");
        System.out.println("1. Realizar Depósito");
        System.out.println("2. Realizar Retiro");
        System.out.println("3. Realizar Transferencia");
        System.out.println("4. Volver al Menú Principal");
        System.out.print("Seleccione una operación: ");
        try {
            return Integer.parseInt(scanner.nextLine());
        } catch (NumberFormatException e) {
            return -1;
        }
    }

    public double pedirImporte() {
        System.out.print("Ingrese el importe (ej. 150.50): ");
        try {
            return Double.parseDouble(scanner.nextLine());
        } catch (NumberFormatException e) {
            return -1;
        }
    }

    public String pedirCuentaDestino() {
        System.out.print("Ingrese el código de la cuenta destino: ");
        return scanner.nextLine();
    }

    public void mostrarResultadoOperacion(boolean exito, String mensaje) {
        if (exito) {
            System.out.println("\n[+] Operación Exitosa: " + mensaje);
        } else {
            System.out.println("\n[-] Error en la operación: " + mensaje);
        }
    }

    public void cerrar() {
        scanner.close();
    }
}