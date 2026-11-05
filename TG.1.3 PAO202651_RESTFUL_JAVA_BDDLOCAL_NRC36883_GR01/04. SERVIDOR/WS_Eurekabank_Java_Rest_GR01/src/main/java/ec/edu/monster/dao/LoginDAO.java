package ec.edu.monster.dao;

import ec.edu.monster.db.AccesoDB;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

/**
 *
 * @author Daniel Guaman <adguaman4@espe.edu.ec>
 */
public class LoginDAO {

    public boolean iniciarSesion(String nombreUsuario, String contrasena) {
        String sql = "SELECT COUNT(*) AS conteo FROM Usuario "
                   + "WHERE vch_emplusuario = ? AND vch_emplclave = SHA(?) AND vch_emplestado = 'ACTIVO'";
        
        try (Connection cn = AccesoDB.getConnection(); 
             PreparedStatement pstm = cn.prepareStatement(sql)) {
            
            pstm.setString(1, nombreUsuario);
            pstm.setString(2, contrasena);

            try (ResultSet rs = pstm.executeQuery()) {
                if (rs.next()) {
                    return rs.getInt("conteo") > 0;
                }
            }
        } catch (SQLException e) {
            throw new RuntimeException("Error al validar usuario: " + e.getMessage());
        }
        return false;
    }
}