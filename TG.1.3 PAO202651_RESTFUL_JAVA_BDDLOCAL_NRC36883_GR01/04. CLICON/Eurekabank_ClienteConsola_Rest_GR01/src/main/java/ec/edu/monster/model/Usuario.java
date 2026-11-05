package ec.edu.monster.model;

/**
 *
 * @author Daniel Guaman <adguaman4@espe.edu.ec>
 */
public class Usuario {
    private String username;
    private String password;
    private boolean autenticado;

    public Usuario(String username, String password) {
        this.username = username;
        this.password = password;
        this.autenticado = false;
    }

    // Getters y Setters
    public String getUsername() { return username; }
    public void setUsername(String username) { this.username = username; }
    public String getPassword() { return password; }
    public void setPassword(String password) { this.password = password; }
    public boolean isAutenticado() { return autenticado; }
    public void setAutenticado(boolean autenticado) { this.autenticado = autenticado; }
}
