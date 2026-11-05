package ec.edu.monster.vista;

import ec.edu.monster.controladores.BancoController;

/**
 *
 * @author Daniel Guaman <adguaman4@espe.edu.ec>
 */
public class Main {
    public static void main(String[] args) {
        ConsolaView vista = new ConsolaView();
        BancoController controlador = new BancoController(vista);
        controlador.iniciar();
    }
}