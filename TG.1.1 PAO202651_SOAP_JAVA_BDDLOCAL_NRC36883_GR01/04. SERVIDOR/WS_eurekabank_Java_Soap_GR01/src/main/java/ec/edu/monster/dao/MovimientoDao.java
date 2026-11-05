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

public class MovimientoDao {

    private static final String TIPO_DEPOSITO = "003";
    private static final String TIPO_RETIRO = "004";
    private static final String TIPO_TRANSF_INGRESO = "008";
    private static final String TIPO_TRANSF_SALIDA = "009";

    public List<MovimientoData> leerMovimientos(String cuenta) {
        List<MovimientoData> lista = new ArrayList<>();
        Map<String, List<MovimientoData>> grupos = new HashMap<>();

        StringBuilder sql = new StringBuilder();
        sql.append("SELECT ")
                .append("m.chr_cuencodigo AS cuenta, ")
                .append("m.int_movinumero AS numero, ")
                .append("m.dtt_movifecha AS fecha, ")
                .append("t.vch_tipodescripcion AS descripcion, ")
                .append("t.vch_tipoaccion AS accion, ")
                .append("m.dec_moviimporte AS importe, ")
                .append("m.chr_cuenreferencia AS referencia, ")
                .append("cl.vch_cliepaterno, ")
                .append("cl.vch_cliematerno, ")
                .append("cl.vch_clienombre ")
                .append("FROM Movimiento m ")
                .append("INNER JOIN TipoMovimiento t ON m.chr_tipocodigo = t.chr_tipocodigo ")
                .append("INNER JOIN Cuenta c ON m.chr_cuencodigo = c.chr_cuencodigo ")
                .append("INNER JOIN Cliente cl ON c.chr_cliecodigo = cl.chr_cliecodigo ");

        if (cuenta != null && !cuenta.trim().isEmpty()) {
            sql.append("WHERE m.chr_cuencodigo = ? ");
        }
        sql.append("ORDER BY m.dtt_movifecha ASC, m.int_movinumero ASC");

        try (Connection cn = AccesoDB.getConnection(); PreparedStatement pstm = cn.prepareStatement(sql.toString())) {
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
                    String tipoFinal = "INGRESO".equals(accion) ? tipoBase + " Entrada" : tipoBase + " Salida";
                    md.setTipo(tipoFinal);

                    String ref = rs.getString("referencia");
                    md.setReferencia(ref != null && !ref.trim().isEmpty() ? ref.trim() : null);

                    grupos.computeIfAbsent(cuentaKey, k -> new ArrayList<>()).add(md);
                }
            }

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
                    if (fechaComp != 0) return fechaComp;
                    return Integer.compare(b.getNumero(), a.getNumero());
                });

                lista.addAll(grupo);
            }
        } catch (SQLException e) {
            throw new RuntimeException("Error al leer movimientos: " + e.getMessage(), e);
        }

        return lista;
    }

    public void realizarMovimiento(String cuentaOrigen, double importe, String codEmp, String tipo, String cuentaDestino) {
        if (importe <= 0) {
            throw new RuntimeException("El importe debe ser mayor a 0.");
        }

        Connection cn = null;
        try {
            cn = AccesoDB.getConnection();
            cn.setAutoCommit(false);

            validarEmpleado(cn, codEmp);

            boolean esDeposito = "DEPOSITO".equalsIgnoreCase(tipo);
            boolean esRetiro = "RETIRO".equalsIgnoreCase(tipo);
            boolean esTransferencia = "TRANSFERENCIA".equalsIgnoreCase(tipo);

            if (!esDeposito && !esRetiro && !esTransferencia) {
                throw new RuntimeException("Tipo inválido: " + tipo);
            }

            if (esTransferencia && (cuentaDestino == null || cuentaDestino.trim().isEmpty())) {
                throw new RuntimeException("Cuenta destino requerida");
            }

            if (esDeposito || esRetiro) {
                boolean esIngreso = esDeposito;
                String tipoCodigo = esDeposito ? TIPO_DEPOSITO : TIPO_RETIRO;
                procesarCuenta(cn, cuentaOrigen, importe, codEmp, tipoCodigo, esIngreso, null);
            }

            if (esTransferencia) {
                if (cuentaOrigen.equals(cuentaDestino)) {
                    throw new RuntimeException("No se puede transferir a la misma cuenta.");
                }
                procesarCuenta(cn, cuentaOrigen, importe, codEmp, TIPO_TRANSF_SALIDA, false, cuentaDestino);
                procesarCuenta(cn, cuentaDestino, importe, codEmp, TIPO_TRANSF_INGRESO, true, cuentaOrigen);
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

        String sql = "SELECT dec_cuensaldo, int_cuencontmov, vch_cuenestado "
                + "FROM Cuenta WHERE chr_cuencodigo = ? FOR UPDATE";
        try (PreparedStatement pstm = cn.prepareStatement(sql)) {
            pstm.setString(1, cuenta);
            try (ResultSet rs = pstm.executeQuery()) {
                if (!rs.next()) {
                    throw new SQLException("Cuenta no existe: " + cuenta);
                }

                String estado = rs.getString("vch_cuenestado");
                if (!"ACTIVO".equals(estado)) {
                    throw new SQLException("Cuenta no está activa: " + cuenta);
                }

                double saldo = rs.getDouble("dec_cuensaldo");
                int cont = rs.getInt("int_cuencontmov");

                if (!esIngreso && saldo < importe) {
                    throw new SQLException("Saldo insuficiente en cuenta: " + cuenta);
                }

                int nroMov = obtenerSiguienteMovimiento(cn, cuenta);
                double nuevoSaldo = esIngreso ? saldo + importe : saldo - importe;
                cont++;

                String updateSql = "UPDATE Cuenta SET dec_cuensaldo = ?, int_cuencontmov = ? WHERE chr_cuencodigo = ?";
                try (PreparedStatement updatePstm = cn.prepareStatement(updateSql)) {
                    updatePstm.setDouble(1, nuevoSaldo);
                    updatePstm.setInt(2, cont);
                    updatePstm.setString(3, cuenta);
                    updatePstm.executeUpdate();
                }

                String insertSql = "INSERT INTO Movimiento(chr_cuencodigo, int_movinumero, dtt_movifecha, "
                        + "chr_emplcodigo, chr_tipocodigo, dec_moviimporte, chr_cuenreferencia) "
                        + "VALUES(?, ?, CURDATE(), ?, ?, ?, ?)";
                try (PreparedStatement insertPstm = cn.prepareStatement(insertSql)) {
                    insertPstm.setString(1, cuenta);
                    insertPstm.setInt(2, nroMov);
                    insertPstm.setString(3, codEmp);
                    insertPstm.setString(4, tipoCodigo);
                    insertPstm.setDouble(5, importe);
                    insertPstm.setString(6, cuentaRef);
                    insertPstm.executeUpdate();
                }
            }
        }
    }

    private int obtenerSiguienteMovimiento(Connection cn, String cuenta) throws SQLException {
        String sql = "SELECT COALESCE(MAX(int_movinumero), 0) + 1 AS nro FROM Movimiento WHERE chr_cuencodigo = ?";
        try (PreparedStatement pstm = cn.prepareStatement(sql)) {
            pstm.setString(1, cuenta);
            try (ResultSet rs = pstm.executeQuery()) {
                if (rs.next()) {
                    return rs.getInt("nro");
                }
            }
        }
        return 1;
    }

    private void validarEmpleado(Connection cn, String codEmp) throws SQLException {
        String sql = "SELECT vch_emplpaterno FROM Empleado WHERE chr_emplcodigo = ?";
        try (PreparedStatement pstm = cn.prepareStatement(sql)) {
            pstm.setString(1, codEmp);
            try (ResultSet rs = pstm.executeQuery()) {
                if (!rs.next()) {
                    throw new SQLException("Empleado no existe: " + codEmp);
                }
            }
        }

        String sql2 = "SELECT vch_emplestado FROM Usuario WHERE chr_emplcodigo = ? AND vch_emplestado = 'ACTIVO'";
        try (PreparedStatement pstm = cn.prepareStatement(sql2)) {
            pstm.setString(1, codEmp);
            try (ResultSet rs = pstm.executeQuery()) {
                if (!rs.next()) {
                    throw new SQLException("Empleado no está activo o no tiene usuario: " + codEmp);
                }
            }
        }
    }

    private void rollback(Connection cn) {
        if (cn != null) {
            try { cn.rollback(); } catch (Exception e) {}
        }
    }

    private void close(Connection cn) {
        if (cn != null) {
            try { cn.close(); } catch (Exception e) {}
        }
    }
}