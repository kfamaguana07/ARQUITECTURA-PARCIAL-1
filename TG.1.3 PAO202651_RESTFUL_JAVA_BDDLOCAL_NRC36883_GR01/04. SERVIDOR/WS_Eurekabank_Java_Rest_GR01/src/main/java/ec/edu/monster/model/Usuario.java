package ec.edu.monster.model;

/**
 *
 * @author Daniel Guaman <adguaman4@espe.edu.ec>
 */
public class Usuario {

    private String nombreUsuario;
    private String contrasena;

    public Usuario() {

    }

    public Usuario(String nombreUsuario, String contrasena) {
        this.nombreUsuario = nombreUsuario;
        this.contrasena = contrasena;
    }

    public String getUsuario() {
        return nombreUsuario;
    }

    public void setUsuario(String nombreUsuario) {
        this.nombreUsuario = nombreUsuario;
    }

    public String getContrasena() {
        return contrasena;
    }

    public void setContrasena(String contrasena) {
        this.contrasena = contrasena;
    }
}
