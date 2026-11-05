package ec.edu.monster.prueba;

import ec.edu.monster.dao.MovimientoDAO;

/**
 *
 * @author Daniel Guaman <adguaman4@espe.edu.ec>
 */
public class PruebaRealizarMovimientos {

    public static void main(String[] args) {
        // Proceso: Usamos el DAO
        MovimientoDAO movimientoDAO = new MovimientoDAO();
        
        try {
            final String DEFAULT_COD_EMPLEADO = "0001";
            String cuentaA = "00100001";
            String cuentaB = "00100002";
            double importe = 100.00;

            System.out.println("=== PRUEBA DE MOVIMIENTOS (SALDO NEUTRO) ===");
            
            // ---------------------------------------------------------
            // 1. PRUEBA DEPÓSITO Y RETIRO
            // ---------------------------------------------------------
            System.out.println("\n[1] Realizando depósito de " + importe + " en cuenta " + cuentaA + "...");
            movimientoDAO.realizarMovimiento(cuentaA, importe, DEFAULT_COD_EMPLEADO, "DEPOSITO", null);
            System.out.println("    -> Depósito OK");

            System.out.println("[2] Realizando retiro de " + importe + " en cuenta " + cuentaA + " para nivelar...");
            movimientoDAO.realizarMovimiento(cuentaA, importe, DEFAULT_COD_EMPLEADO, "RETIRO", null);
            System.out.println("    -> Retiro OK");

            // ---------------------------------------------------------
            // 2. PRUEBA TRANSFERENCIA (IDA Y VUELTA)
            // ---------------------------------------------------------
            System.out.println("\n[3] Realizando transferencia de IDA de " + cuentaA + " a " + cuentaB + " por " + importe + "...");
            movimientoDAO.realizarMovimiento(cuentaA, importe, DEFAULT_COD_EMPLEADO, "TRANSFERENCIA", cuentaB);
            System.out.println("    -> Transferencia de ida OK");

            System.out.println("[4] Realizando transferencia de VUELTA de " + cuentaB + " a " + cuentaA + " por " + importe + "...");
            movimientoDAO.realizarMovimiento(cuentaB, importe, DEFAULT_COD_EMPLEADO, "TRANSFERENCIA", cuentaA);
            System.out.println("    -> Transferencia de vuelta OK");

            System.out.println("\n=== PRUEBA FINALIZADA CON ÉXITO ===");
            System.out.println("Los saldos de las cuentas " + cuentaA + " y " + cuentaB + " no han sido afectados.");

        } catch (Exception e) {
            System.out.println("Error en la operación: " + e.getMessage());
            e.printStackTrace();
        }
    }
}