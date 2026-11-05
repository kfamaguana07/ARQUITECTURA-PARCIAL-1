package ec.edu.monster.prueba;

import ec.edu.monster.service.Login;

/**
 *
 * @author daniel
 */
public class PruebaLogin {

    public static void main(String[] args) {
        try {
            // Datos de prueba reales según DATOS_PARA_BDD.sql
            String usuario = "cromero"; // Carlos Romero
            String contrasena = "chicho"; // Su contraseña en texto plano
            
            // Proceso
            Login service = new Login();
            if (service.IniciarSesion(usuario, contrasena)) {
                System.out.println("Logeado exitosamente");
            } else {
                System.out.println("Credenciales incorrectas");
            }
        } catch (Exception e) {
            System.out.println("Error en el login: " + e.getMessage());
            e.printStackTrace();
        }
    }
}