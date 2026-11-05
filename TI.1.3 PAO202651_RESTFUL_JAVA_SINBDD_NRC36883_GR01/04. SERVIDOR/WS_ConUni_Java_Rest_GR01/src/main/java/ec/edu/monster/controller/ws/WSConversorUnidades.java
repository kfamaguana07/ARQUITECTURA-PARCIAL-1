package ec.edu.monster.controller.ws;

import ec.edu.monster.controller.ws.dto.ConversionRequest;
import ec.edu.monster.controller.ws.dto.ConversionResponse;
import ec.edu.monster.controller.ws.dto.MensajeResponse;
import ec.edu.monster.model.servicios.ServicioConversor;
import ec.edu.monster.model.utilidades.ConversorLongitud;
import ec.edu.monster.model.utilidades.ConversorMasa;
import ec.edu.monster.model.utilidades.ConversorTemperatura;
import ec.edu.monster.model.utilidades.enums.UnidadLongitud;
import ec.edu.monster.model.utilidades.enums.UnidadMasa;
import ec.edu.monster.model.utilidades.enums.UnidadTemperatura;
import ec.edu.monster.model.utilidades.mapeadores.UnidadMapper;
import jakarta.ws.rs.Consumes;
import jakarta.ws.rs.POST;
import jakarta.ws.rs.Path;
import jakarta.ws.rs.Produces;
import jakarta.ws.rs.core.MediaType;
import jakarta.ws.rs.core.Response;

@Path("/conversiones")
@Consumes(MediaType.APPLICATION_JSON)
@Produces(MediaType.APPLICATION_JSON)
public class WSConversorUnidades {

    private final ServicioConversor<UnidadLongitud> servicioLongitud =
            new ServicioConversor<>(new ConversorLongitud());

    private final ServicioConversor<UnidadMasa> servicioMasa =
            new ServicioConversor<>(new ConversorMasa());

    private final ServicioConversor<UnidadTemperatura> servicioTemperatura =
            new ServicioConversor<>(new ConversorTemperatura());

    @POST
    @Path("/longitud")
    public Response convertirLongitud(ConversionRequest request) {
        try {
            validarDatosConversion(request);
            UnidadLongitud origen = UnidadMapper.toLongitud(request.getUnidadOrigen());
            UnidadLongitud destino = UnidadMapper.toLongitud(request.getUnidadDestino());

            double resultado = servicioLongitud.convertir(request.getValor(), origen, destino);
            return Response.ok(crearRespuesta(request, resultado)).build();
        } catch (RuntimeException e) {
            return construirRespuestaError(Response.Status.BAD_REQUEST, e.getMessage());
        }
    }

    @POST
    @Path("/masa")
    public Response convertirMasa(ConversionRequest request) {
        try {
            validarDatosConversion(request);
            UnidadMasa origen = UnidadMapper.toMasa(request.getUnidadOrigen());
            UnidadMasa destino = UnidadMapper.toMasa(request.getUnidadDestino());

            double resultado = servicioMasa.convertir(request.getValor(), origen, destino);
            return Response.ok(crearRespuesta(request, resultado)).build();
        } catch (RuntimeException e) {
            return construirRespuestaError(Response.Status.BAD_REQUEST, e.getMessage());
        }
    }

    @POST
    @Path("/temperatura")
    public Response convertirTemperatura(ConversionRequest request) {
        try {
            validarDatosConversion(request);
            UnidadTemperatura origen = UnidadMapper.toTemperatura(request.getUnidadOrigen());
            UnidadTemperatura destino = UnidadMapper.toTemperatura(request.getUnidadDestino());

            double resultado = servicioTemperatura.convertir(request.getValor(), origen, destino);
            return Response.ok(crearRespuesta(request, resultado)).build();
        } catch (RuntimeException e) {
            return construirRespuestaError(Response.Status.BAD_REQUEST, e.getMessage());
        }
    }

    private static void validarDatosConversion(ConversionRequest request) {
        if (request == null
                || request.getValor() == null
                || request.getUnidadOrigen() == null
                || request.getUnidadOrigen().isBlank()
                || request.getUnidadDestino() == null
                || request.getUnidadDestino().isBlank()) {
            throw new RuntimeException("Valor y unidades son obligatorios");
        }
    }

    private static ConversionResponse crearRespuesta(ConversionRequest request, double valorConvertido) {
        ConversionResponse respuesta = new ConversionResponse();
        respuesta.setValorOriginal(request.getValor());
        respuesta.setUnidadOrigen(request.getUnidadOrigen());
        respuesta.setValorConvertido(valorConvertido);
        respuesta.setUnidadDestino(request.getUnidadDestino());
        return respuesta;
    }

    private static Response construirRespuestaError(Response.Status estado, String mensaje) {
        return Response.status(estado)
                .entity(new MensajeResponse(mensaje))
                .type(MediaType.APPLICATION_JSON)
                .build();
    }
}
