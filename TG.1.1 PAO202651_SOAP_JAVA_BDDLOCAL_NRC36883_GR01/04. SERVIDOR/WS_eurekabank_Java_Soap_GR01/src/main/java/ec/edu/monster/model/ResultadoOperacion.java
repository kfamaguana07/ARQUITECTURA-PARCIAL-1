package ec.edu.monster.model;


import jakarta.xml.bind.annotation.XmlRootElement;
import jakarta.xml.bind.annotation.XmlElement;
/**
 *
 * @author daniel
 */


@XmlRootElement(name = "resultado")
public class ResultadoOperacion {

    private int codigo;        // 1 = éxito, -1 = error
    private String mensaje;    // Descripción clara

    public ResultadoOperacion() {}

    public ResultadoOperacion(int codigo, String mensaje) {
        this.codigo = codigo;
        this.mensaje = mensaje;
    }

    @XmlElement
    public int getCodigo() { return codigo; }
    public void setCodigo(int codigo) { this.codigo = codigo; }

    @XmlElement
    public String getMensaje() { return mensaje; }
    public void setMensaje(String mensaje) { this.mensaje = mensaje; }

    // Métodos de ayuda
    public static ResultadoOperacion exito() {
        return new ResultadoOperacion(1, "Operación exitosa");
    }

    public static ResultadoOperacion error(String msg) {
        return new ResultadoOperacion(-1, msg);
    }
}