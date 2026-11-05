package ec.edu.monster.service;

import ec.edu.monster.dao.CuentaDao;
import ec.edu.monster.dao.MovimientoDao;
import ec.edu.monster.model.DatosCuenta;
import ec.edu.monster.model.MovimientoData;
import java.util.List;

public class EurekaService {

    private final CuentaDao cuentaDao = new CuentaDao();
    private final MovimientoDao movimientoDao = new MovimientoDao();

    public List<DatosCuenta> leerCuentasConClientes() {
        return cuentaDao.leerCuentasConClientes();
    }

    public List<DatosCuenta> leerCuentasPorCliente(String clienteCodigo) {
        return cuentaDao.leerCuentasPorCliente(clienteCodigo);
    }

    public List<MovimientoData> leerMovimientos(String cuenta) {
        return movimientoDao.leerMovimientos(cuenta);
    }

    public void registrarDeposito(String cuenta, double importe, String codEmp) {
        movimientoDao.realizarMovimiento(cuenta, importe, codEmp, "DEPOSITO", null);
    }

    public void registrarRetiro(String cuenta, double importe, String codEmp) {
        movimientoDao.realizarMovimiento(cuenta, importe, codEmp, "RETIRO", null);
    }

    public void registrarTransferencia(String cuentaOrigen, String cuentaDestino, double importe, String codEmp) {
        movimientoDao.realizarMovimiento(cuentaOrigen, importe, codEmp, "TRANSFERENCIA", cuentaDestino);
    }
}