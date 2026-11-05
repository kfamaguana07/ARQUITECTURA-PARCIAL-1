using ec.edu.monster.Mapeadores;
using ec.edu.monster.Servicios;
using ec.edu.monster.Utilidades;
using ec.edu.monster.Utilidades.Enums;

namespace ec.edu.monster.Controladores
{
    // Controlador de aplicacion para encapsular la logica de conversion (patron MVC interno).
    public class ConversionesControlador
    {
        private readonly ServicioConversor<UnidadLongitud> servicioLongitud =
            new ServicioConversor<UnidadLongitud>(new ConversorLongitud());

        private readonly ServicioConversor<UnidadMasa> servicioMasa =
            new ServicioConversor<UnidadMasa>(new ConversorMasa());

        private readonly ServicioConversor<UnidadTemperatura> servicioTemperatura =
            new ServicioConversor<UnidadTemperatura>(new ConversorTemperatura());

        public double ConvertirLongitud(double valor, string unidadInicial, string unidadFinal)
        {
            var origen = UnidadMapper.ToLongitud(unidadInicial);
            var destino = UnidadMapper.ToLongitud(unidadFinal);
            return servicioLongitud.Convertir(valor, origen, destino);
        }

        public double ConvertirMasa(double valor, string unidadInicial, string unidadFinal)
        {
            var origen = UnidadMapper.ToMasa(unidadInicial);
            var destino = UnidadMapper.ToMasa(unidadFinal);
            return servicioMasa.Convertir(valor, origen, destino);
        }

        public double ConvertirTemperatura(double valor, string unidadInicial, string unidadFinal)
        {
            var origen = UnidadMapper.ToTemperatura(unidadInicial);
            var destino = UnidadMapper.ToTemperatura(unidadFinal);
            return servicioTemperatura.Convertir(valor, origen, destino);
        }
    }
}
