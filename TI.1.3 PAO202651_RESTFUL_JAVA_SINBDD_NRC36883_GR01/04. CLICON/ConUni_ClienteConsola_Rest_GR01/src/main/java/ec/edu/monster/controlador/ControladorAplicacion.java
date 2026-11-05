package ec.edu.monster.controlador;

import ec.edu.monster.modelos.ModeloClienteRest;

public class ControladorAplicacion {

    // El controlador es el único que conoce la existencia del modelo de red
    private final ModeloClienteRest modeloRest;

    public ControladorAplicacion() {
        this.modeloRest = new ModeloClienteRest();
    }

    // --- MÉTODOS DE SEGURIDAD ---

    public void autenticar(String usuario, String contrasenia) throws Exception {
        modeloRest.iniciarSesion(usuario, contrasenia);
    }

    public void cambiarContrasenia(String contraseniaActual, String contraseniaNueva) throws Exception {
        modeloRest.cambiarContrasenia(contraseniaActual, contraseniaNueva);
        // Regla de negocio: Forzamos el cierre de sesión tras un cambio exitoso
        modeloRest.cerrarSesion(); 
    }

    public void cerrarSesion() {
        modeloRest.cerrarSesion();
    }

    // --- MÉTODOS DE CONVERSIÓN ---

    public double convertir(String tipoMagnitud, double valor, String unidadOrigen, String unidadDestino) throws Exception {
        // Delegamos el cálculo al modelo que se comunicará con el backend .NET / Java
        return modeloRest.convertirMagnitud(tipoMagnitud, valor, unidadOrigen, unidadDestino);
    }
}