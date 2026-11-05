package ec.edu.monster.modelos;

import java.net.URI;
import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class ModeloClienteRest {

    // Contexto real del WAR desplegado en GlassFish/Payara: WS_ConUni_Java_Rest_GR01
    private static final String URL_BASE = "http://localhost:8080/WS_ConUni_Java_Rest_GR01/api";
    private final HttpClient clienteHttp;
    private String tokenSesion;

    public ModeloClienteRest() {
        this.clienteHttp = HttpClient.newHttpClient();
    }

    public boolean estaAutenticado() {
        return tokenSesion != null;
    }

    public void cerrarSesion() {
        this.tokenSesion = null;
    }

    public void iniciarSesion(String usuario, String contrasenia) throws Exception {
        // Respetamos @JsonbProperty de tu API
        String jsonPeticion = String.format("{\"Usuario\": \"%s\", \"Contrasena\": \"%s\"}", usuario, contrasenia);

        HttpRequest peticion = HttpRequest.newBuilder()
                .uri(URI.create(URL_BASE + "/seguridad/login"))
                .header("Content-Type", "application/json")
                .POST(HttpRequest.BodyPublishers.ofString(jsonPeticion))
                .build();

        HttpResponse<String> respuesta = clienteHttp.send(peticion, HttpResponse.BodyHandlers.ofString());

        if (respuesta.statusCode() == 200) {
            String token = extraerValorJson(respuesta.body(), "token", false);
            if (token == null || token.isBlank()) {
                throw new Exception("El servidor respondió sin token de sesión.");
            }
            this.tokenSesion = token;
        } else if (respuesta.statusCode() == 401) {
            throw new Exception("Credenciales incorrectas.");
        } else if (respuesta.statusCode() == 404) {
            throw new Exception("Ruta no encontrada (404). Verifique el contexto del servidor: " + URL_BASE);
        } else {
            throw new Exception("Error de servidor (" + respuesta.statusCode() + ").");
        }
    }

    public void cambiarContrasenia(String contraseniaActual, String contraseniaNueva) throws Exception {
        String jsonPeticion = String.format("{\"ContrasenaActual\": \"%s\", \"NuevaContrasena\": \"%s\"}", contraseniaActual, contraseniaNueva);

        HttpRequest peticion = HttpRequest.newBuilder()
                .uri(URI.create(URL_BASE + "/seguridad/cambiar-contrasena"))
                .header("Content-Type", "application/json")
                .header("Authorization", "Bearer " + this.tokenSesion)
                .POST(HttpRequest.BodyPublishers.ofString(jsonPeticion))
                .build();

        HttpResponse<String> respuesta = clienteHttp.send(peticion, HttpResponse.BodyHandlers.ofString());

        if (respuesta.statusCode() != 200) {
            throw new Exception("No se pudo cambiar la contraseña (" + respuesta.statusCode() + ")");
        }
    }

    public double convertirMagnitud(String tipoMagnitud, double valor, String unidadOrigen, String unidadDestino) throws Exception {
        String jsonPeticion = String.format(
            "{\"Valor\": %s, \"UnidadOrigen\": \"%s\", \"UnidadDestino\": \"%s\"}", 
            String.valueOf(valor).replace(",", "."), unidadOrigen, unidadDestino
        );

        HttpRequest peticion = HttpRequest.newBuilder()
                .uri(URI.create(URL_BASE + "/conversiones/" + tipoMagnitud))
                .header("Content-Type", "application/json")
                .header("Authorization", "Bearer " + this.tokenSesion)
                .POST(HttpRequest.BodyPublishers.ofString(jsonPeticion))
                .build();

        HttpResponse<String> respuesta = clienteHttp.send(peticion, HttpResponse.BodyHandlers.ofString());

        if (respuesta.statusCode() == 200) {
            String resultadoStr = extraerValorJson(respuesta.body(), "valorConvertido", true);
            if (resultadoStr == null) {
                throw new Exception("Respuesta del servidor sin el campo ValorConvertido.");
            }
            return Double.parseDouble(resultadoStr);
        } else {
            throw new Exception("Error en la conversión: Verifica las unidades ingresadas.");
        }
    }

    // Utilidad interna para parsear JSON sin librerías pesadas
    private String extraerValorJson(String json, String clave, boolean esNumero) {
        if (json == null) {
            return null;
        }
        // Acepta negativos y notación exponencial (ej. -40.0 o 1.5E-3)
        String patron = esNumero
            ? "\"" + clave + "\"\\s*:\\s*(-?[0-9]+(?:\\.[0-9]+)?(?:[eE][+-]?[0-9]+)?)"
            : "\"" + clave + "\"\\s*:\\s*\"([^\"]+)\"";
        Matcher buscador = Pattern.compile(patron, Pattern.CASE_INSENSITIVE).matcher(json);
        return buscador.find() ? buscador.group(1) : null;
    }
}