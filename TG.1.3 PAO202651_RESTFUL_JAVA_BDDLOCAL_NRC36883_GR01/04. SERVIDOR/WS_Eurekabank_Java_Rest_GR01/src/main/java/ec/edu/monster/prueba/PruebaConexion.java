
package ec.edu.monster.prueba;

import ec.edu.monster.db.AccesoDB;
import java.sql.Connection;

/**
 *
 * @author Daniel Guaman <adguaman4@espe.edu.ec>
 */
public class PruebaConexion {

    public static void main(String[] args) {
        try (Connection cn = AccesoDB.getConnection()) {
            System.out.println("Conexion exitosa a la base de datos!");
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

}
