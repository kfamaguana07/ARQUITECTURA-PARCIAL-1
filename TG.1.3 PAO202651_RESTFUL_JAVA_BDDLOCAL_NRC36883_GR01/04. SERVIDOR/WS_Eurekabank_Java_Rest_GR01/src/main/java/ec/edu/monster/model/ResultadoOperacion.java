package ec.edu.monster.model;

/**
 *
 * @author Daniel Guaman <adguaman4@espe.edu.ec>
 */
public class ResultadoOperacion {

    private int codigo;        // 1 = éxito, -1 = error
    private String mensaje;    // Descripción clara

    public ResultadoOperacion() {}

    public ResultadoOperacion(int codigo, String mensaje) {
        this.codigo = codigo;
        this.mensaje = mensaje;
    }

    public int getCodigo() { return codigo; }
    public void setCodigo(int codigo) { this.codigo = codigo; }

    
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