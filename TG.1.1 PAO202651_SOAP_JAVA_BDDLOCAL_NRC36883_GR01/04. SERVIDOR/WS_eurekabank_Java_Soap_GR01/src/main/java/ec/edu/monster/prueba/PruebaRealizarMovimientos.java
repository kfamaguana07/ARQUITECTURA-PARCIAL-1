package ec.edu.monster.prueba;

import ec.edu.monster.model.MovimientoData;
import ec.edu.monster.service.EurekaService;
import java.util.List;

/**
 *
 * @author daniel
 */
public class PruebaRealizarMovimientos {

    public static void main(String[] args) {
        EurekaService service = new EurekaService();
        
        try {
            System.out.println("=== PRUEBA DE ESCRITURA Y LECTURA (SIN ROLLBACK) ===");
            
            // Datos de la prueba
            String cuenta = "00100001"; // Cuenta de Alan Aranda
            String codEmpleado = "0001"; // Carlos Romero
            double importe = 100.00; // Monto a depositar

            // 1. Leemos los movimientos ANTES de la operación
            List<MovimientoData> movsAntes = service.leerMovimientos(cuenta);
            System.out.println("Movimientos antes: " + movsAntes.size());
            if (!movsAntes.isEmpty()) {
                System.out.println("Saldo antes: " + movsAntes.get(0).getSaldoActual());
            }

            // 2. Realizamos el depósito
            System.out.println("\nRegistrando depósito de " + importe + " en la cuenta " + cuenta + "...");
            service.registrarDeposito(cuenta, importe, codEmpleado);
            System.out.println("¡Depósito registrado!");

            // 3. Leemos los movimientos DESPUÉS de la operación
            System.out.println("\n--- MOVIMIENTOS DESPUÉS DEL DEPÓSITO ---");
            List<MovimientoData> movsDespues = service.leerMovimientos(cuenta);
            
            // Imprimimos todos los movimientos (del más reciente al más antiguo)
            for (MovimientoData mov : movsDespues) {
                System.out.println("Nro: " + mov.getNumero() + 
                                   " | Fecha: " + mov.getFecha() + 
                                   " | Tipo: " + mov.getTipo() + 
                                   " | Importe: " + mov.getImporte() + 
                                   " | Saldo Acumulado: " + mov.getSaldoActual());
            }

            System.out.println("\nTotal de movimientos ahora: " + movsDespues.size());

        } catch (Exception e) {
            System.out.println("Error en la prueba: " + e.getMessage());
            e.printStackTrace();
        }
    }
}