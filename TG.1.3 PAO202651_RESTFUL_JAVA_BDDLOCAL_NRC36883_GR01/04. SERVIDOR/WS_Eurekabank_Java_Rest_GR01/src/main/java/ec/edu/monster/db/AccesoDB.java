package ec.edu.monster.db;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.SQLException;

/**
 *
 * @author Daniel Guaman <adguaman4@espe.edu.ec>
 */
public class AccesoDB {

    private AccesoDB() {
    }

    public static Connection getConnection() throws SQLException {
        Connection cn = null;

        try {
            // Driver oficial de MySQL
            String driver = "com.mysql.cj.jdbc.Driver";
            
            // URL apuntando a MySQL, puerto 3306, base de datos eureka_bank
            // Se agregan los parámetros que configuraste en Payara: UTC y useSSL=false
            String url = "jdbc:mysql://localhost:3306/eureka_bank?useSSL=false&serverTimezone=UTC";
            
            String user = "eureka";
            String pass = "admin";

            Class.forName(driver);
            cn = DriverManager.getConnection(url, user, pass);

        } catch (ClassNotFoundException e) {
            throw new SQLException("ERROR: no se encuentra el driver JDBC de MySQL.");
        } catch (SQLException e) {
            throw new SQLException("ERROR al conectar: " + e.getMessage());
        }

        return cn;
    }

    public static Connection getConnectionModern() throws SQLException {
        Connection cn = null;

        try {
            cn = DriverManager.getConnection(
                    "jdbc:mysql://localhost:3306/eureka_bank?useSSL=false&serverTimezone=UTC",
                    "eureka",
                    "admin"
            );

        } catch (SQLException e) {
            throw new SQLException("ERROR al conectar: " + e.getMessage());
        }

        return cn;
    }

}