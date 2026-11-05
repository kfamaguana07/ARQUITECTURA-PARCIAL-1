package ec.edu.monster.controller.ws.dto;

import jakarta.json.bind.annotation.JsonbProperty;

public class ConversionResponse {

    @JsonbProperty("ValorOriginal")
    private double valorOriginal;

    @JsonbProperty("UnidadOrigen")
    private String unidadOrigen;

    @JsonbProperty("ValorConvertido")
    private double valorConvertido;

    @JsonbProperty("UnidadDestino")
    private String unidadDestino;

    public double getValorOriginal() {
        return valorOriginal;
    }

    public void setValorOriginal(double valorOriginal) {
        this.valorOriginal = valorOriginal;
    }

    public String getUnidadOrigen() {
        return unidadOrigen;
    }

    public void setUnidadOrigen(String unidadOrigen) {
        this.unidadOrigen = unidadOrigen;
    }

    public double getValorConvertido() {
        return valorConvertido;
    }

    public void setValorConvertido(double valorConvertido) {
        this.valorConvertido = valorConvertido;
    }

    public String getUnidadDestino() {
        return unidadDestino;
    }

    public void setUnidadDestino(String unidadDestino) {
        this.unidadDestino = unidadDestino;
    }
}
