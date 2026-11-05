package ec.edu.monster.vistas;

import ec.edu.monster.model.*;
import ec.edu.monster.servicios.EuBankService;
import java.util.Scanner;

/**
 *
 * @author Daniel Guaman <adguaman4@espe.edu.ec>
 */
public class LoginVista {

    private final EuBankService euBankService;
    private final Scanner scanner;

    public LoginVista(EuBankService euBankService) {
        this.euBankService = euBankService;
        this.scanner = new Scanner(System.in, "UTF-8");
    }

    public Usuario mostrarLogin() {
        limpiarPantalla();
        System.out.println("\n╔══════════════════════════════════════════╗");
        System.out.println("║  Sistema de Login - EuBank (REST)   ║");
        System.out.println("╚══════════════════════════════════════════╝");

        int intentos = 0;
        final int MAX_INTENTOS = 3;

        while (intentos < MAX_INTENTOS) {
            System.out.print("Usuario: ");
            String username = scanner.nextLine().trim();
            if (username.isEmpty()) {
                System.out.println("El usuario no puede estar vacío.");
                continue;
            }

            System.out.print("Contraseña: ");
            String password = scanner.nextLine().trim();
            if (password.isEmpty()) {
                System.out.println("La contraseña no puede estar vacía.");
                continue;
            }

            System.out.print("\nAutenticando (REST)...");
            Usuario usuario = new Usuario(username, password);
            try {
                // --- LÓGICA DE LOGIN REST ---
                ResultadoOperacion resultado = euBankService.iniciarSesion(usuario.getUsername(), usuario.getPassword());
                
                if (resultado.getCodigo() == 1) {
                    System.out.println(" ✓");
                    System.out.println("\nAutenticación exitosa. Bienvenido, " + username + "!");
                    usuario.setAutenticado(true);
                    esperarTecla();
                    return usuario;
                } else {
                    System.out.println(" ✗");
                    intentos++;
                    mostrarError(resultado.getMensaje() + " (Intento " + intentos + "/" + MAX_INTENTOS + ")");
                }
            } catch (Exception e) {
                System.out.println(" ✗");
                intentos++;
                mostrarError("Error inesperado: " + e.getMessage() + " (Intento " + intentos + "/" + MAX_INTENTOS + ")");
            }
        }
        return null;
    }

    private void mostrarError(String mensaje) {
        System.out.print("\u001B[31m");
        System.out.println("\n✗ " + mensaje);
        System.out.print("\u001B[0m");
    }

    private void esperarTecla() {
        System.out.println("\nPresione Enter para continuar...");
        try { scanner.nextLine(); } catch (Exception e) {}
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