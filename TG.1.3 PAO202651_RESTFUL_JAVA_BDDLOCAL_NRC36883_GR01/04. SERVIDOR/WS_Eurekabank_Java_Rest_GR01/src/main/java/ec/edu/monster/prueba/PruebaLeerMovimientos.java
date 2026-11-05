package ec.edu.monster.prueba;

import ec.edu.monster.dao.MovimientoDAO; 
import ec.edu.monster.model.MovimientoData;
import java.util.List;

/**
 *
 * @author Daniel Guaman <adguaman4@espe.edu.ec>
 */
public class PruebaLeerMovimientos {

    public static void main(String[] args) {
        try {
            // Dato de la prueba
            String cuenta = "00100001";
            
            // Proceso: Usamos el DAO directamente
            MovimientoDAO movimientoDAO = new MovimientoDAO();
            List<MovimientoData> lista = movimientoDAO.leerMovimientos(cuenta);
            
            // Reporte
            System.out.println("=== MOVIMIENTOS DE LA CUENTA " + cuenta + " ===");
            System.out.println("Nro | Fecha | Tipo | Importe | Referencia");
            System.out.println("--------------------------------------------------");
            
            for (MovimientoData r : lista) {
                // Corregido el error de imprimir r.getNumero() dos veces
                System.out.println(r.getNumero() + " - " 
                        + r.getFecha() + " - " 
                        + r.getTipo() + " - " 
                        + r.getImporte() + " - " 
                        + (r.getReferencia() != null ? r.getReferencia() : "N/A"));
            }
            
        } catch (Exception e) {
            System.out.println("Error: " + e.getMessage());
            e.printStackTrace();
        }
    }
}