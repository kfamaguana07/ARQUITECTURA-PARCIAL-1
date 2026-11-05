package ec.edu.monster.controller.ws;

import ec.edu.monster.controller.manejadoresws.ManejadorAutenticacion;
import java.util.HashSet;
import java.util.Set;
import jakarta.ws.rs.ApplicationPath;
import jakarta.ws.rs.core.Application;

@ApplicationPath("/api")
public class ConfiguracionRest extends Application {

    @Override
    public Set<Class<?>> getClasses() {
        Set<Class<?>> clasesRegistradas = new HashSet<>();
        clasesRegistradas.add(WSConversorUnidades.class);
        clasesRegistradas.add(WSSeguridad.class);
        clasesRegistradas.add(ManejadorAutenticacion.class);
        return clasesRegistradas;
    }
}