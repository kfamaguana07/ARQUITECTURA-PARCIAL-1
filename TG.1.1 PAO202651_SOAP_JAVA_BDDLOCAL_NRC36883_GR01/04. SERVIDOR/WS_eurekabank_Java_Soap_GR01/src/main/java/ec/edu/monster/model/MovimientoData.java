package ec.edu.monster.model;

import jakarta.xml.bind.annotation.*;
import java.math.BigDecimal;
import java.util.Date;

/**
 *
 * @author daniel
 */
@XmlRootElement(name = "MovimientoData")
@XmlAccessorType(XmlAccessType.FIELD)
public class MovimientoData {

    @XmlElement(name = "CodigoCuenta")
    private String codigoCuenta;

    @XmlElement(name = "Numero")
    private int numero;

    @XmlElement(name = "Fecha")
    private Date fecha;

    @XmlElement(name = "Tipo")
    private String tipo; // Ej: "Transferencia Entrada"

    @XmlElement(name = "Importe")
    private BigDecimal importe;

    @XmlElement(name = "Referencia")
    private String referencia; // opcional

    @XmlElement(name = "SaldoActual")
    private BigDecimal saldoActual;

    @XmlElement(name = "NombreCliente")
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
