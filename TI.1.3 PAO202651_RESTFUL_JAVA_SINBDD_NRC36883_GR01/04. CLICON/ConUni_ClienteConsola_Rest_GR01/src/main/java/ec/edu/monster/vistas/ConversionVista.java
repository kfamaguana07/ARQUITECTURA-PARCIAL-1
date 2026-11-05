package ec.edu.monster.vistas;

import ec.edu.monster.controlador.ControladorAplicacion;
import java.util.Scanner;

public class ConversionVista {

    private static final String[] UNIDADES_LONGITUD = {"MILIMETRO", "CENTIMETRO", "METRO", "KILOMETRO", "YARDA"};
    private static final String[] UNIDADES_MASA = {"MILIGRAMO", "GRAMO", "KILOGRAMO", "TONELADA", "ONZA"};
    private static final String[] UNIDADES_TEMPERATURA = {"CELSIUS", "FAHRENHEIT", "KELVIN", "RANKINE"};

    private final ControladorAplicacion controlador;
    private final Scanner escaner;

    public ConversionVista(ControladorAplicacion controlador, Scanner escaner) {
        this.controlador = controlador;
        this.escaner = escaner;
    }

    public void ejecutarMenu() {
        int opcion;
        do {
            System.out.println("\n=== MENÚ PRINCIPAL (JAVA REST) ===");
            System.out.println("1. Convertir Longitud");
            System.out.println("2. Convertir Masa");
            System.out.println("3. Convertir Temperatura");
            System.out.println("4. Cambiar Contraseña");
            System.out.println("0. Salir y Cerrar Sesión");
            System.out.print("Seleccione una opción: ");

            opcion = leerEntero();

            switch (opcion) {
                case 1 -> procesarConversion("longitud", UNIDADES_LONGITUD);
                case 2 -> procesarConversion("masa", UNIDADES_MASA);
                case 3 -> procesarConversion("temperatura", UNIDADES_TEMPERATURA);
                case 4 -> cambiarContrasenia();
                case 0 -> System.out.println("Saliendo del menú de conversión...");
                default -> System.out.println("Opción no válida. Intente nuevamente.");
            }
        } while (opcion != 0);
    }

    private void procesarConversion(String tipoMagnitud, String[] unidadesPermitidas) {
        System.out.println("\n--- CONVERSIÓN DE " + tipoMagnitud.toUpperCase() + " ---");

        System.out.print("Ingrese el valor a convertir: ");
        double valor = leerDecimal();

        String unidadInicial = solicitarUnidad("inicial", unidadesPermitidas);
        String unidadFinal = solicitarUnidad("final", unidadesPermitidas);

        try {
            // Delegamos todo el cálculo y la red al controlador
            double resultado = controlador.convertir(tipoMagnitud, valor, unidadInicial, unidadFinal);
            System.out.printf("Resultado: %.4f %s = %.4f %s\n", valor, unidadInicial, resultado, unidadFinal);
        } catch (Exception ex) {
            System.out.println("ERROR: " + ex.getMessage());
        }
    }

    private String solicitarUnidad(String tipoUnidad, String[] unidadesPermitidas) {
        System.out.println("Seleccione la unidad " + tipoUnidad + ":");
        for (int i = 0; i < unidadesPermitidas.length; i++) {
            System.out.println((i + 1) + ". " + unidadesPermitidas[i]);
        }

        while (true) {
            System.out.print("Opción de unidad: ");
            int opcion = leerEntero();
            if (opcion >= 1 && opcion <= unidadesPermitidas.length) {
                return unidadesPermitidas[opcion - 1]; 
            }
            System.out.println("Unidad no válida. Elija una opción de la lista.");
        }
    }

    private void cambiarContrasenia() {
        String nueva = "";
        String nueva_conf = "";
        System.out.print("\nContraseña actual: ");
        String actual = escaner.nextLine();
        while (true) {
            System.out.print("Nueva contraseña (o '-1' para cancelar): ");
            nueva = escaner.nextLine();

            if (nueva.equals("-1")) {
                System.out.println("Operación cancelada. Regresando al menú...");
                return;
            }

            System.out.print("Confirmar contraseña: ");
            nueva_conf = escaner.nextLine();

            if (nueva.equals(nueva_conf)) {
                break; 
            } else {
                System.out.println("Las contraseñas no coinciden. Intente nuevamente.\n");
            }
        }

        try {
            // Delegamos el cambio al controlador
            controlador.cambiarContrasenia(actual, nueva);
            System.out.println("Contraseña actualizada correctamente.");
            System.out.println("Por seguridad, cierre su sesion y vuelva a ingresar.");
            
        } catch (Exception ex) {
            System.out.println("ERROR: " + ex.getMessage());
        }
    }

    // --- MÉTODOS UTILITARIOS DE LECTURA ---

    private int leerEntero() {
        while (!escaner.hasNextInt()) {
            System.out.print("Por favor, ingrese un número válido: ");
            escaner.next();
        }
        int valor = escaner.nextInt();
        escaner.nextLine(); // Limpiar el buffer
        return valor;
    }

    private double leerDecimal() {
        while (!escaner.hasNextDouble()) {
            System.out.print("Por favor, ingrese un decimal válido: ");
            escaner.next();
        }
        double valor = escaner.nextDouble();
        escaner.nextLine(); // Limpiar el buffer
        return valor;
    }
}