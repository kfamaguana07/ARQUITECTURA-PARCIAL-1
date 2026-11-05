package ec.edu.monster.controller.ws.dto;

import jakarta.json.bind.annotation.JsonbProperty;

public class LoginResponse {

    @JsonbProperty("Token")
    private String token;

    public LoginResponse() {
    }

    public LoginResponse(String token) {
        this.token = token;
    }

    public String getToken() {
        return token;
    }

    public void setToken(String token) {
        this.token = token;
    }
}
