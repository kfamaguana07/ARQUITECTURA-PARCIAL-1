using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WS_ConUni_DotNet_Rest_GR5.Modelos;
using WS_ConUni_DotNet_Rest_GR5.Modelos.Servicios;

namespace WS_ConUni_DotNet_Rest_GR5.Controladores;

[ApiController]
[Route("api/conversiones")]
[Authorize(AuthenticationSchemes = "BearerToken")]
public class ConversionesController : ControllerBase
{
    private readonly ServicioConversion _servicioConversion;

    public ConversionesController(ServicioConversion servicioConversion)
    {
        _servicioConversion = servicioConversion;
    }

    [HttpPost("longitud")]
    public ActionResult<ConversionResponse> ConvertirLongitud([FromBody] ConversionRequest request)
    {
        try
        {
            var resultado = _servicioConversion.ConvertirLongitud(request.Valor, request.UnidadOrigen, request.UnidadDestino);
            return Ok(CrearRespuesta(request, resultado));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("temperatura")]
    public ActionResult<ConversionResponse> ConvertirTemperatura([FromBody] ConversionRequest request)
    {
        try
        {
            var resultado = _servicioConversion.ConvertirTemperatura(request.Valor, request.UnidadOrigen, request.UnidadDestino);
            return Ok(CrearRespuesta(request, resultado));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("masa")]
    public ActionResult<ConversionResponse> ConvertirMasa([FromBody] ConversionRequest request)
    {
        try
        {
            var resultado = _servicioConversion.ConvertirMasa(request.Valor, request.UnidadOrigen, request.UnidadDestino);
            return Ok(CrearRespuesta(request, resultado));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    private static ConversionResponse CrearRespuesta(ConversionRequest request, double valorConvertido)
    {
        return new ConversionResponse
        {
            ValorOriginal = request.Valor,
            UnidadOrigen = request.UnidadOrigen,
            ValorConvertido = valorConvertido,
            UnidadDestino = request.UnidadDestino
        };
    }
}
