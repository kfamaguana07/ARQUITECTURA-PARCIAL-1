package ec.edu.monster.controller.ws.dto;

import jakarta.json.bind.annotation.JsonbProperty;

public class LoginRequest {

    @JsonbProperty("Usuario")
    private String usuario;

    @JsonbProperty("Contrasena")
    private String contrasenia;

    public String getUsuario() {
        return usuario;
    }

    public void setUsuario(String usuario) {
        this.usuario = usuario;
    }

    public String getContrasenia() {
        return contrasenia;
    }

    public void setContrasenia(String contrasenia) {
        this.contrasenia = contrasenia;
    }
}
