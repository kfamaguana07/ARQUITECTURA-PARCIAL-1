package ec.edu.monster.controller.ws.dto;

import jakarta.json.bind.annotation.JsonbProperty;

public class ConversionRequest {

    @JsonbProperty("Valor")
    private Double valor;

    @JsonbProperty("UnidadOrigen")
    private String unidadOrigen;

    @JsonbProperty("UnidadDestino")
    private String unidadDestino;

    public Double getValor() {
        return valor;
    }

    public void setValor(Double valor) {
        this.valor = valor;
    }

    public String getUnidadOrigen() {
        return unidadOrigen;
    }

    public void setUnidadOrigen(String unidadOrigen) {
        this.unidadOrigen = unidadOrigen;
    }

    public String getUnidadDestino() {
        return unidadDestino;
    }

    public void setUnidadDestino(String unidadDestino) {
        this.unidadDestino = unidadDestino;
    }
}
