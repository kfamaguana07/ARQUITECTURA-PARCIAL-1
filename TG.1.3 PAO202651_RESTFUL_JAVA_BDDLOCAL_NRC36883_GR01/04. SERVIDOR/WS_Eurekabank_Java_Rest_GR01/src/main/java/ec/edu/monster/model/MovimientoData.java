package ec.edu.monster.model;

import java.math.BigDecimal;
import java.util.Date;

/**
 *
 * @author Daniel Guaman <adguaman4@espe.edu.ec>
 */
public class MovimientoData {

    private String codigoCuenta;

    private int numero;

    private Date fecha;

    private String tipo; // Ej: "Transferencia Entrada"

    private BigDecimal importe;

    private String referencia; // opcional

    private BigDecimal saldoActual;

    private String nombreCliente;

    // Constructor vacío
    public MovimientoData() {
    }

    // Getters y Setters
    public String getCodigoCuenta() {
        return codigoCuenta;
    }

    public void setCodigoCuenta(String codigoCuenta) {
        this.codigoCuenta = codigoCuenta;
    }

    public int getNumero() {
        return numero;
    }

    public void setNumero(int numero) {
        this.numero = numero;
    }

    public Date getFecha() {
        return fecha;
    }

    public void setFecha(Date fecha) {
        this.fecha = fecha;
    }

    public String getTipo() {
        return tipo;
    }

    public void setTipo(String tipo) {
        this.tipo = tipo;
    }

    public BigDecimal getImporte() {
        return importe;
    }

    public void setImporte(BigDecimal importe) {
        this.importe = importe;
    }

    public String getReferencia() {
        return referencia;
    }

    public void setReferencia(String referencia) {
        this.referencia = referencia;
    }

    public BigDecimal getSaldoActual() {
        return saldoActual;
    }

    public void setSaldoActual(BigDecimal saldoActual) {
        this.saldoActual = saldoActual;
    }

    public String getNombreCliente() {
        return nombreCliente;
    }

    public void setNombreCliente(String nombreCliente) {
        this.nombreCliente = nombreCliente;
    }
}
