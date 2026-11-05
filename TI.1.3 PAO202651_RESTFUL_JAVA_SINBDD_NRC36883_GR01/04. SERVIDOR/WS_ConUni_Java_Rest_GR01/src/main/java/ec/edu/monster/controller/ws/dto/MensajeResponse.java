package ec.edu.monster.controller.ws.dto;

import jakarta.json.bind.annotation.JsonbProperty;

public class MensajeResponse {

    @JsonbProperty("mensaje")
    private String mensaje;

    public MensajeResponse() {
    }

    public MensajeResponse(String mensaje) {
        this.mensaje = mensaje;
    }

    public String getMensaje() {
        return mensaje;
    }

    public void setMensaje(String mensaje) {
        this.mensaje = mensaje;
    }
}
