package ec.edu.monster.dao;

import ec.edu.monster.db.AccesoDB;
import ec.edu.monster.model.MovimientoData;
import java.math.BigDecimal;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 *
 * @author Daniel Guaman <adguaman4@espe.edu.ec>
 */
public class MovimientoDAO {

    // --- LECTURA ---
    public List<MovimientoData> leerMovimientos(String cuenta) {
        List<MovimientoData> lista = new ArrayList<>();
        Map<String, List<MovimientoData>> grupos = new HashMap<>();

        StringBuilder sql = new StringBuilder();
        sql.append("SELECT m.chr_cuencodigo AS cuenta, m.int_movinumero AS numero, ")
           .append("m.dtt_movifecha AS fecha, t.vch_tipodescripcion AS descripcion, ")
           .append("t.vch_tipoaccion AS accion, m.dec_moviimporte AS importe, ")
           .append("m.chr_cuenreferencia AS referencia, cl.vch_cliepaterno, ")
           .append("cl.vch_cliematerno, cl.vch_clienombre ")
           .append("FROM Movimiento m ")
           .append("INNER JOIN TipoMovimiento t ON m.chr_tipocodigo = t.chr_tipocodigo ")
           .append("INNER JOIN Cuenta c ON m.chr_cuencodigo = c.chr_cuencodigo ")
           .append("INNER JOIN Cliente cl ON c.chr_cliecodigo = cl.chr_cliecodigo ");

        if (cuenta != null && !cuenta.trim().isEmpty()) {
            sql.append("WHERE m.chr_cuencodigo = ? ");
        }
        sql.append("ORDER BY m.dtt_movifecha ASC, m.int_movinumero ASC");

        try (Connection cn = AccesoDB.getConnection(); 
             PreparedStatement pstm = cn.prepareStatement(sql.toString())) {

            if (cuenta != null && !cuenta.trim().isEmpty()) {
                pstm.setString(1, cuenta);
            }

            try (ResultSet rs = pstm.executeQuery()) {
                while (rs.next()) {
                    MovimientoData md = new MovimientoData();
                    String cuentaKey = rs.getString("cuenta");
                    md.setCodigoCuenta(cuentaKey);
                    md.setNumero(rs.getInt("numero"));
                    md.setFecha(rs.getDate("fecha"));
                    md.setImporte(rs.getBigDecimal("importe"));

                    String nombre = (rs.getString("vch_cliepaterno") + " "
                            + rs.getString("vch_cliematerno") + ", "
                            + rs.getString("vch_clienombre")).trim();
                    md.setNombreCliente(nombre);

                    String accion = rs.getString("accion");
                    String tipoBase = rs.getString("descripcion");
                    md.setTipo("INGRESO".equals(accion) ? tipoBase + " Entrada" : tipoBase + " Salida");

                    String ref = rs.getString("referencia");
                    md.setReferencia(ref != null && !ref.trim().isEmpty() ? ref.trim() : null);

                    grupos.computeIfAbsent(cuentaKey, k -> new ArrayList<>()).add(md);
                }
            }

            // Calcular saldo acumulado
            for (List<MovimientoData> grupo : grupos.values()) {
                BigDecimal saldoAcumulado = BigDecimal.ZERO;
                for (MovimientoData md : grupo) {
                    String accion = md.getTipo().contains("Entrada") ? "INGRESO" : "SALIDA";
                    BigDecimal importe = md.getImporte();
                    BigDecimal movimiento = "INGRESO".equals(accion) ? importe : importe.negate();
                    saldoAcumulado = saldoAcumulado.add(movimiento);
                    md.setSaldoActual(saldoAcumulado);
                }

                grupo.sort((a, b) -> {
                    int fechaComp = b.getFecha().compareTo(a.getFecha());
                    return fechaComp != 0 ? fechaComp : Integer.compare(b.getNumero(), a.getNumero());
                });
                lista.addAll(grupo);
            }
        } catch (SQLException e) {
            throw new RuntimeException("Error al leer movimientos: " + e.getMessage(), e);
        }
        return lista;
    }

    // --- ESCRITURA (Transacciones) ---
    public void realizarMovimiento(String cuentaOrigen, double importe, String codEmp, String tipo, String cuentaDestino) {
        if (importe <= 0) throw new RuntimeException("El importe debe ser mayor a 0.");

        Connection cn = null;
        try {
            cn = AccesoDB.getConnection();
            cn.setAutoCommit(false);

            boolean esDeposito = "DEPOSITO".equalsIgnoreCase(tipo);
            boolean esRetiro = "RETIRO".equalsIgnoreCase(tipo);
            boolean esTransferencia = "TRANSFERENCIA".equalsIgnoreCase(tipo);

            if (!esDeposito && !esRetiro && !esTransferencia) throw new RuntimeException("Tipo inválido: " + tipo);
            if (esTransferencia && (cuentaDestino == null || cuentaDestino.trim().isEmpty())) throw new RuntimeException("Cuenta destino requerida");

            if (esDeposito || esRetiro) {
                procesarCuenta(cn, cuentaOrigen, importe, codEmp, esDeposito ? "003" : "004", esDeposito, null);
            } else if (esTransferencia) {
                if (cuentaOrigen.equals(cuentaDestino)) throw new RuntimeException("No se puede transferir a la misma cuenta.");
                procesarCuenta(cn, cuentaOrigen, importe, codEmp, "009", false, cuentaDestino);
                procesarCuenta(cn, cuentaDestino, importe, codEmp, "008", true, cuentaOrigen);
            }

            cn.commit();
        } catch (SQLException e) {
            rollback(cn);
            throw new RuntimeException("Error en BD: " + e.getMessage());
        } catch (Exception e) {
            rollback(cn);
            throw new RuntimeException("Error: " + e.getMessage());
        } finally {
            close(cn);
        }
    }

    private void procesarCuenta(Connection cn, String cuenta, double importe, String codEmp,
            String tipoCodigo, boolean esIngreso, String cuentaRef) throws SQLException {

        String sql = "SELECT dec_cuensaldo, int_cuencontmov, vch_cuenestado FROM Cuenta WHERE chr_cuencodigo = ? FOR UPDATE";
        try (PreparedStatement pstm = cn.prepareStatement(sql)) {
            pstm.setString(1, cuenta);
            try (ResultSet rs = pstm.executeQuery()) {
                if (!rs.next()) throw new SQLException("Cuenta no existe: " + cuenta);
                if (!"ACTIVO".equals(rs.getString("vch_cuenestado"))) throw new SQLException("Cuenta no está activa: " + cuenta);

                double saldo = rs.getDouble("dec_cuensaldo");
                int cont = rs.getInt("int_cuencontmov");

                if (!esIngreso && saldo < importe) throw new SQLException("Saldo insuficiente en cuenta: " + cuenta);

                int nroMov = obtenerSiguienteMovimiento(cn, cuenta);
                double nuevoSaldo = esIngreso ? saldo + importe : saldo - importe;
                cont++;

                String sqlUpdate = "UPDATE Cuenta SET dec_cuensaldo = ?, int_cuencontmov = ? WHERE chr_cuencodigo = ?";
                try (PreparedStatement pstmUpd = cn.prepareStatement(sqlUpdate)) {
                    pstmUpd.setDouble(1, nuevoSaldo);
                    pstmUpd.setInt(2, cont);
                    pstmUpd.setString(3, cuenta);
                    pstmUpd.executeUpdate();
                }

                String sqlInsert = "INSERT INTO Movimiento(chr_cuencodigo, int_movinumero, dtt_movifecha, chr_emplcodigo, chr_tipocodigo, dec_moviimporte, chr_cuenreferencia) VALUES(?, ?, CURDATE(), ?, ?, ?, ?)";
                try (PreparedStatement pstmIns = cn.prepareStatement(sqlInsert)) {
                    pstmIns.setString(1, cuenta);
                    pstmIns.setInt(2, nroMov);
                    pstmIns.setString(3, codEmp);
                    pstmIns.setString(4, tipoCodigo);
                    pstmIns.setDouble(5, importe);
                    pstmIns.setString(6, cuentaRef);
                    pstmIns.executeUpdate();
                }
            }
        }
    }

    private int obtenerSiguienteMovimiento(Connection cn, String cuenta) throws SQLException {
        String sql = "SELECT COALESCE(MAX(int_movinumero), 0) + 1 AS nro FROM Movimiento WHERE chr_cuencodigo = ?";
        try (PreparedStatement pstm = cn.prepareStatement(sql)) {
            pstm.setString(1, cuenta);
            try (ResultSet rs = pstm.executeQuery()) {
                return rs.next() ? rs.getInt("nro") : 1;
            }
        }
    }

    private void rollback(Connection cn) { if (cn != null) try { cn.rollback(); } catch (Exception e) {} }
    private void close(Connection cn) { if (cn != null) try { cn.close(); } catch (Exception e) {} }

}
