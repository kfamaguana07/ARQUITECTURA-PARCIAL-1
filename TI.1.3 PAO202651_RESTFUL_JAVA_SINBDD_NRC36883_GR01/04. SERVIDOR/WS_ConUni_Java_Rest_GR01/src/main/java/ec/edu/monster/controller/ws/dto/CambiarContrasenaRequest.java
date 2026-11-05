package ec.edu.monster.controller.ws.dto;

import jakarta.json.bind.annotation.JsonbProperty;

public class CambiarContrasenaRequest {

    @JsonbProperty("ContrasenaActual")
    private String contraseniaActual;

    @JsonbProperty("NuevaContrasena")
    private String nuevaContrasenia;

    public String getContraseniaActual() {
        return contraseniaActual;
    }

    public void setContraseniaActual(String contraseniaActual) {
        this.contraseniaActual = contraseniaActual;
    }

    public String getNuevaContrasenia() {
        return nuevaContrasenia;
    }

    public void setNuevaContrasenia(String nuevaContrasenia) {
        this.nuevaContrasenia = nuevaContrasenia;
    }
}
