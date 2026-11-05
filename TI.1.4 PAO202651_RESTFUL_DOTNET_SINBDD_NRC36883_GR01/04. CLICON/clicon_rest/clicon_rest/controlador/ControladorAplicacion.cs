using ec.edu.monster.Modelos;

namespace ec.edu.monster.Controladores;

public class ControladorAplicacion
{
    private readonly ModeloClienteRest _modeloRest;

    public ControladorAplicacion()
    {
        _modeloRest = new ModeloClienteRest();
    }

    public async Task AutenticarAsync(string usuario, string contrasena)
    {
        await _modeloRest.IniciarSesionAsync(usuario, contrasena);
    }

    public async Task CambiarContrasenaAsync(string contrasenaActual, string contrasenaNueva)
    {
        await _modeloRest.CambiarContrasenaAsync(contrasenaActual, contrasenaNueva);
        _modeloRest.CerrarSesion();
    }

    public void CerrarSesion()
    {
        _modeloRest.CerrarSesion();
    }

    public async Task<double> ConvertirAsync(string tipoMagnitud, double valor, string unidadOrigen, string unidadDestino)
    {
        return await _modeloRest.ConvertirMagnitudAsync(tipoMagnitud, valor, unidadOrigen, unidadDestino);
    }
}