package ec.edu.monster.prueba;

import ec.edu.monster.model.MovimientoData;
import ec.edu.monster.service.EurekaService;
import java.util.List;

/**
 *
 * @author daniel
 */
public class PruebaLeerMovimientos {

    public static void main(String[] args) {
        EurekaService service = new EurekaService();
        
        try {
            System.out.println("=== PRUEBA DE LECTURA DE MOVIMIENTOS ===");
            
            // Cuenta de prueba según el script SQL (pertenece a Alan Aranda)
            String cuenta = "00100001";

            // Ejecutamos la lectura de movimientos
            List<MovimientoData> movimientos = service.leerMovimientos(cuenta);

            if (movimientos.isEmpty()) {
                System.out.println("No se encontraron movimientos para la cuenta " + cuenta);
            } else {
                System.out.println("Movimientos encontrados para la cuenta " + cuenta + ":\n");
                
                for (MovimientoData mov : movimientos) {
                    System.out.println("Nro Mov: " + mov.getNumero());
                    System.out.println("Fecha:   " + mov.getFecha());
                    System.out.println("Cliente: " + mov.getNombreCliente());
                    System.out.println("Tipo:    " + mov.getTipo());
                    System.out.println("Importe: " + mov.getImporte());
                    System.out.println("Saldo:   " + mov.getSaldoActual()); // Saldo acumulado
                    
                    if (mov.getReferencia() != null) {
                        System.out.println("Ref:     " + mov.getReferencia());
                    }
                    System.out.println("----------------------------------------");
                }
            }

        } catch (Exception e) {
            System.out.println("Error en la consulta: " + e.getMessage());
            e.printStackTrace();
        }
    }
}