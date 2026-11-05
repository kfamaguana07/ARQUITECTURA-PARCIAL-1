package ec.edu.monster.prueba;

import ec.edu.monster.dao.LoginDAO;

/**
 *
 * @author Daniel Guaman <adguaman4@espe.edu.ec>
 */
public class PruebaLogin {

    public static void main(String[] args) {
        try {
            // Datos de prueba reales según DATOS_PARA_BDD.sql
            String usuario = "cromero"; 
            String contrasena = "chicho"; 
            
            // Proceso: Usamos el DAO
            LoginDAO loginDAO = new LoginDAO();
            
            if (loginDAO.iniciarSesion(usuario, contrasena)) {
                System.out.println("Logeado exitosamente");
            } else {
                System.out.println("Credenciales incorrectas");
            }
        } catch (Exception e) {
            System.out.println("Error en login: " + e.getMessage());
            e.printStackTrace();
        }
    }
}