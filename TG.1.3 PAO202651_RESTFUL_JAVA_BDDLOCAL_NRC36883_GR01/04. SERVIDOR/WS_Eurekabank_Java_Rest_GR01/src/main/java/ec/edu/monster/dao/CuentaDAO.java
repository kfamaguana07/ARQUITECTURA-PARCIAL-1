package ec.edu.monster.dao;

import ec.edu.monster.db.AccesoDB;
import ec.edu.monster.model.DatosCuenta;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.List;

/**
 *
 * @author Daniel Guaman <adguaman4@espe.edu.ec>
 */

public class CuentaDAO {

    public List<DatosCuenta> leerCuentasConClientes() {
        List<DatosCuenta> lista = new ArrayList<>();
        String sql = "SELECT "
                + "c.chr_cuencodigo, c.chr_monecodigo, c.dec_cuensaldo, c.vch_cuenestado, "
                + "cl.vch_cliepaterno, cl.vch_cliematerno, cl.vch_clienombre, cl.vch_clieemail, cl.vch_clietelefono "
                + "FROM Cuenta c "
                + "INNER JOIN Cliente cl ON c.chr_cliecodigo = cl.chr_cliecodigo "
                + "WHERE c.vch_cuenestado = 'ACTIVO' "
                + "ORDER BY c.chr_cuencodigo";

        try (Connection cn = AccesoDB.getConnection(); 
             PreparedStatement pstm = cn.prepareStatement(sql); 
             ResultSet rs = pstm.executeQuery()) {

            while (rs.next()) {
                DatosCuenta dc = new DatosCuenta();
                dc.setCodigo(rs.getString("chr_cuencodigo"));
                dc.setMoneda(rs.getString("chr_monecodigo"));
                dc.setSaldo(rs.getBigDecimal("dec_cuensaldo"));
                dc.setEstado(rs.getString("vch_cuenestado"));
                
                String nombreCompleto = (rs.getString("vch_cliepaterno") + " "
                        + rs.getString("vch_cliematerno") + ", "
                        + rs.getString("vch_clienombre")).trim();
                dc.setNombreCliente(nombreCompleto);
                dc.setEmailCliente(rs.getString("vch_clieemail"));
                dc.setTelefonoCliente(rs.getString("vch_clietelefono"));

                lista.add(dc);
            }
        } catch (SQLException e) {
            throw new RuntimeException("Error al obtener cuentas con clientes: " + e.getMessage(), e);
        }
        return lista;
    }
}