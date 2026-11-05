package ec.edu.monster.model;

/**
 *
 * @author daniel
 */

import jakarta.xml.bind.annotation.XmlElement;
import jakarta.xml.bind.annotation.XmlType;

@XmlType(name = "MovimientoRequest")
public class MovimientoRequestDto {
    private String cuentaOrigen;
    private String cuentaDestino;
    private String tipo;
    private double importe;

    @XmlElement(required = true)
    public String getCuentaOrigen() { return cuentaOrigen; }
    public void setCuentaOrigen(String cuentaOrigen) { this.cuentaOrigen = cuentaOrigen; }

    @XmlElement(required = false)
    public String getCuentaDestino() { return cuentaDestino; }
    public void setCuentaDestino(String cuentaDestino) { this.cuentaDestino = cuentaDestino; }

    @XmlElement(required = true)
    public String getTipo() { return tipo; }
    public void setTipo(String tipo) { this.tipo = tipo; }

    @XmlElement(required = true)
    public double getImporte() { return importe; }
    public void setImporte(double importe) { this.importe = importe; }
}
