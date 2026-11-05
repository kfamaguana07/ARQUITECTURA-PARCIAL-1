/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 */

package ec.edu.monster;

import ec.edu.monster.controlador.ControladorAplicacion;
import ec.edu.monster.vistas.LoginVista;
import java.io.PrintStream;
import java.nio.charset.StandardCharsets;

/**
 *
 * @author ariel
 */
public class Clicon {

    public static void main(String[] args) {
        System.setOut(new PrintStream(System.out, true, StandardCharsets.UTF_8));
        ControladorAplicacion controladorCentral = new ControladorAplicacion();
        
        LoginVista vistaLogin = new LoginVista(controladorCentral);
        
        vistaLogin.iniciar();
    }
}
