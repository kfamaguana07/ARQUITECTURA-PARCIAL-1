package ec.edu.monster.vistas;

import ec.edu.monster.controlador.ControladorAplicacion;
import java.util.Scanner;

public class LoginVista {

    private final ControladorAplicacion controlador;
    private final Scanner escaner;

    public LoginVista(ControladorAplicacion controlador) {
        this.controlador = controlador;
        this.escaner = new Scanner(System.in);
    }

    public void iniciar() {
        mensajeBienvenida();
        System.out.println("=== SISTEMA DE CONVERSIÓN DE UNIDADES - JAVA REST-GR01 ===");

        while (true) {
            System.out.print("\nUsuario: ");
            String usuario = escaner.nextLine();
            
            System.out.print("Contraseña: ");
            String contrasenia = escaner.nextLine();

            try {
                // Delegamos la validación al controlador
                controlador.autenticar(usuario, contrasenia);
                System.out.println("Inicio de sesión exitoso. Token guardado.");

                // Pasamos el control a la vista de conversión
                ConversionVista vistaConversion = new ConversionVista(controlador, escaner);
                vistaConversion.ejecutarMenu();

                // Al salir del menú de conversión, cerramos sesión de forma segura
                controlador.cerrarSesion();
                System.out.println("Sesión finalizada de forma segura.");
                return; 
                
            } catch (Exception ex) {
                System.out.println("ERROR: " + ex.getMessage());
            }
        }
    }

    private void mensajeBienvenida() {
        String COLOR_CIAN = "\u001B[36m";
        String COLOR_RESET = "\u001B[0m";

        String mensaje = """
         __  __                   _                  
        |  \\/  | ___  _ __   ___ | |_ ___  _ __      
        | |\\/| |/ _ \\| '_ \\ / __|| __/ _ \\| '__|     
        | |  | | (_) | | | |\\__ \\| ||  __/| |        
        |_|  |_|\\___/|_| |_||___/ \\__\\___||_|        """;

        System.out.println(COLOR_CIAN + mensaje + COLOR_RESET);
    }
}