package ec.edu.monster.cliente;

import ec.edu.monster.model.Usuario;
import ec.edu.monster.servicios.EuBankService;
import ec.edu.monster.vistas.LoginVista;
import ec.edu.monster.vistas.DashboardVista;

/**
 *
 * @author Daniel Guaman <adguaman4@espe.edu.ec>
 */
public class ConsoleRunner {

    public static void main(String[] args) {
        // 1. Forzar la salida en UTF-8 para evitar problemas con tildes
        System.setOut(new java.io.PrintStream(System.out, true, java.nio.charset.StandardCharsets.UTF_8));
        System.out.println("--- Cliente REST - EuBank (Consola) GR01 ---");

        try {
            // 2. Instanciar el servicio y las vistas manualmente
            EuBankService service = new EuBankService();
            LoginVista loginVista = new LoginVista(service);
            DashboardVista dashboardVista = new DashboardVista(service); 

            // 3. Flujo: Login -> Dashboard
            Usuario usuario = loginVista.mostrarLogin();

            if (usuario != null && usuario.isAutenticado()) {
                dashboardVista.mostrarMenuPrincipal();
            } else {
                System.out.println("\nNo se pudo autenticar. Saliendo del sistema...");
            }
        } catch (Exception e) {
            mostrarError("Error crítico: " + e.getMessage());
            e.printStackTrace();
        }
        
        System.out.println("\nPresione Enter para salir...");
        try {
            System.in.read();
        } catch (Exception e) {
            // Ignorar
        }
        System.exit(0);
    }
    
    private static void mostrarError(String mensaje) {
         System.out.print("\u001B[31m");
         System.out.println("\n✗ " + mensaje);
         System.out.print("\u001B[0m");
    }
}