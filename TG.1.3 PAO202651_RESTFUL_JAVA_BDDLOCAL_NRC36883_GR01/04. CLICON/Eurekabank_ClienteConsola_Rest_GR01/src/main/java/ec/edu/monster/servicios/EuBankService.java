package ec.edu.monster.servicios;

import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;
import ec.edu.monster.model.DatosCuenta;
import ec.edu.monster.model.MovimientoData;
import ec.edu.monster.model.MovimientoRequestDto;
import ec.edu.monster.model.ResultadoOperacion;

import java.lang.reflect.Type;
import java.net.URI;
import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse;
import java.util.List;

/**
 *
 * @author Daniel Guaman <adguaman4@espe.edu.ec>
 */
public class EuBankService {

    private static final String BASE_URL = "http://localhost:8080/WS_Eurekabank_Java_Rest_GR01/api/eurekabank";
    
    private final HttpClient httpClient;
    private final Gson gson;

    public EuBankService() {
        this.httpClient = HttpClient.newHttpClient();
        this.gson = new Gson();
    }

    // Clase auxiliar para enviar el JSON de login
    private static class LoginRequest {
        String usuario;
        String contrasena;

        public LoginRequest(String usuario, String contrasena) {
            this.usuario = usuario;
            this.contrasena = contrasena;
        }
    }

    /**
     * 1. LOGIN
     */
    public ResultadoOperacion iniciarSesion(String usuario, String contrasena) throws Exception {
        LoginRequest loginRequest = new LoginRequest(usuario, contrasena);
        String jsonBody = gson.toJson(loginRequest);

        HttpRequest request = HttpRequest.newBuilder()
                .uri(URI.create(BASE_URL + "/login"))
                .header("Content-Type", "application/json")
                .POST(HttpRequest.BodyPublishers.ofString(jsonBody))
                .build();

        HttpResponse<String> response = httpClient.send(request, HttpResponse.BodyHandlers.ofString());

        if (response.statusCode() == 200) {
            return gson.fromJson(response.body(), ResultadoOperacion.class);
        } else {
            // Si el servidor responde con 400 o 500, intentamos leer el mensaje de error
            try {
                return gson.fromJson(response.body(), ResultadoOperacion.class);
            } catch (Exception e) {
                return ResultadoOperacion.error("Error en el servidor: " + response.statusCode());
            }
        }
    }

    /**
     * 2. OBTENER CUENTAS
     */
    public List<DatosCuenta> obtenerCuentas() throws Exception {
        HttpRequest request = HttpRequest.newBuilder()
                .uri(URI.create(BASE_URL + "/cuentas"))
                .header("Accept", "application/json")
                .GET()
                .build();

        HttpResponse<String> response = httpClient.send(request, HttpResponse.BodyHandlers.ofString());

        if (response.statusCode() == 200) {
            Type listType = new TypeToken<List<DatosCuenta>>(){}.getType();
            return gson.fromJson(response.body(), listType);
        } else {
            throw new Exception("Error al obtener cuentas. Código: " + response.statusCode());
        }
    }

    /**
     * 3. OBTENER MOVIMIENTOS (Opcional: filtrar por cuenta)
     */
    public List<MovimientoData> obtenerMovimientos(String cuenta) throws Exception {
        String url = BASE_URL + "/movimientos";
        if (cuenta != null && !cuenta.trim().isEmpty()) {
            url += "?cuenta=" + cuenta.trim();
        }

        HttpRequest request = HttpRequest.newBuilder()
                .uri(URI.create(url))
                .header("Accept", "application/json")
                .GET()
                .build();

        HttpResponse<String> response = httpClient.send(request, HttpResponse.BodyHandlers.ofString());

        if (response.statusCode() == 200) {
            Type listType = new TypeToken<List<MovimientoData>>(){}.getType();
            return gson.fromJson(response.body(), listType);
        } else {
            throw new Exception("Error al obtener movimientos. Código: " + response.statusCode());
        }
    }

    /**
     * 4. REGISTRAR MOVIMIENTO (Depósito, Retiro o Transferencia)
     */
    public ResultadoOperacion registrarMovimiento(MovimientoRequestDto dto) throws Exception {
        String jsonBody = gson.toJson(dto);

        HttpRequest request = HttpRequest.newBuilder()
                .uri(URI.create(BASE_URL + "/movimientos"))
                .header("Content-Type", "application/json")
                .POST(HttpRequest.BodyPublishers.ofString(jsonBody))
                .build();

        HttpResponse<String> response = httpClient.send(request, HttpResponse.BodyHandlers.ofString());

        try {
            return gson.fromJson(response.body(), ResultadoOperacion.class);
        } catch (Exception e) {
            if (response.statusCode() == 200) {
                return ResultadoOperacion.exito();
            }
            return ResultadoOperacion.error("Error en la operación. Código: " + response.statusCode());
        }
    }

    /**
     * 5. OBTENER TIPOS DE MOVIMIENTO
     */
    public List<String> obtenerTiposMovimiento() throws Exception {
        HttpRequest request = HttpRequest.newBuilder()
                .uri(URI.create(BASE_URL + "/tipos"))
                .header("Accept", "application/json")
                .GET()
                .build();

        HttpResponse<String> response = httpClient.send(request, HttpResponse.BodyHandlers.ofString());

        if (response.statusCode() == 200) {
            Type listType = new TypeToken<List<String>>(){}.getType();
            return gson.fromJson(response.body(), listType);
        } else {
            throw new Exception("Error al obtener tipos de movimiento. Código: " + response.statusCode());
        }
    }
}