
package ec.edu.monster.prueba;

import ec.edu.monster.db.AccesoDB;
import java.sql.Connection;

/**
 *
 * @author daniel
 */
public class PruebaConexion {

    public static void main(String[] args) {
        try (Connection cn = AccesoDB.getConnection()) {
            System.out.println("✅ Conexión exitosa a la base de datos!");
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

}
