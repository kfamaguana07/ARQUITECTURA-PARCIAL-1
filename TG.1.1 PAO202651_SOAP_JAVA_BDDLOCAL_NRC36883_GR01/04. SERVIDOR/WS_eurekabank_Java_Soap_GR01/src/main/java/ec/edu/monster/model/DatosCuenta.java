package ec.edu.monster.model;

import jakarta.xml.bind.annotation.XmlElement;
import jakarta.xml.bind.annotation.XmlRootElement;
import java.math.BigDecimal;

/**
 *
 * @author daniel
 */

@XmlRootElement(name = "GetDatosCuentaResult")
public class DatosCuenta {

    private String codigo;
    private String nombreCliente;
    private BigDecimal saldo;
    private String estado;
    private String moneda;
    private String emailCliente;
    private String telefonoCliente;

    // Constructor vacío (JAXB)
    public DatosCuenta() {
    }

    // Getters y Setters con @XmlElement para nombres personalizados
    @XmlElement(name = "Codigo")
    public String getCodigo() {
        return codigo;
    }

    public void setCodigo(String codigo) {
        this.codigo = codigo;
    }

    @XmlElement(name = "NombreCliente")
    public String getNombreCliente() {
        return nombreCliente;
    }

    public void setNombreCliente(String nombreCliente) {
        this.nombreCliente = nombreCliente;
    }

    @XmlElement(name = "Saldo")
    public BigDecimal getSaldo() {
        return saldo;
    }

    public void setSaldo(BigDecimal saldo) {
        this.saldo = saldo;
    }

    @XmlElement(name = "Estado")
    public String getEstado() {
        return estado;
    }

    public void setEstado(String estado) {
        this.estado = estado;
    }

    @XmlElement(name = "Moneda")
    public String getMoneda() {
        return moneda;
    }

    public void setMoneda(String moneda) {
        this.moneda = moneda;
    }

    @XmlElement(name = "EmailCliente")
    public String getEmailCliente() {
        return emailCliente;
    }

    public void setEmailCliente(String emailCliente) {
        this.emailCliente = emailCliente;
    }

    @XmlElement(name = "TelefonoCliente")
    public String getTelefonoCliente() {
        return telefonoCliente;
    }

    public void setTelefonoCliente(String telefonoCliente) {
        this.telefonoCliente = telefonoCliente;
    }
}
