package ec.edu.monster.service;

import ec.edu.monster.db.AccesoDB;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

public class Login {

    public boolean IniciarSesion(String nombreUsuario, String contrasena) {
        Connection cn = null;
        boolean isValid = false;
        try {
            cn = AccesoDB.getConnection();
            
            // Usamos la función SHA() de MySQL y validamos el estado
            String sql = "SELECT COUNT(*) AS conteo "
                    + "FROM Usuario "
                    + "WHERE vch_emplusuario = ? AND vch_emplclave = SHA(?) AND vch_emplestado = 'ACTIVO'";
            
            PreparedStatement pstm = cn.prepareStatement(sql);
            pstm.setString(1, nombreUsuario);
            pstm.setString(2, contrasena); // Pasamos la contraseña en texto plano

            ResultSet rs = pstm.executeQuery();
            if (rs.next()) {
                int conteo = rs.getInt("conteo");
                isValid = (conteo > 0); 
            }

            rs.close();
            pstm.close();
        } catch (SQLException e) {
            throw new RuntimeException("Error al validar usuario: " + e.getMessage());
        } finally {
            try {
                if (cn != null) {
                    cn.close();
                }
            } catch (SQLException e) {
                e.printStackTrace(); // Mejor imprimir el error que lanzar otra excepción aquí
            }
        }
        return isValid;
    }
}
