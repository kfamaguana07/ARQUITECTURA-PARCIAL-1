using ec.edu.monster.Cliente.Vistas;
using ec.edu.monster.Controladores;

var controladorCentral = new ControladorAplicacion();

var vistaLogin = new VistaLogin(controladorCentral);

await vistaLogin.IniciarAsync();