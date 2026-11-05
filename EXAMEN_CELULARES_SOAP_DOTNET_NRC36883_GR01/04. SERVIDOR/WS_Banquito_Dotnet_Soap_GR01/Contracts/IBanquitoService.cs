using System.Collections.Generic;
using System.ServiceModel;
using WS_Banquito_Dotnet_Soap_GR01.Models;

namespace WS_Banquito_Dotnet_Soap_GR01.Services
{
    [ServiceContract]
    public interface IBanquitoService
    {
        /// <summary>
        /// Valida si una persona es sujeta de crédito según su número de cédula.
        /// </summary>
        [OperationContract]
        RespuestaValidacion ValidarSujetoCredito(string cedula);

        /// <summary>
        /// Calcula el monto máximo de crédito autorizado para un cliente.
        /// </summary>
        [OperationContract]
        decimal ObtenerMontoMaximo(int codigoCliente);

        /// <summary>
        /// Otorga el crédito, genera y almacena la tabla de amortización en la BD.
        /// Retorna el ID del crédito generado (o 0 si falló).
        /// </summary>
        [OperationContract]
        int OtorgarCredito(int codigoCliente, decimal precioTelefono, int numeroCuotas);

        /// <summary>
        /// Retorna la tabla de amortización guardada para un crédito otorgado.
        /// </summary>
        [OperationContract]
        List<CuotaAmortizacion> ConsultarTablaAmortizacion(int codigoCredito);
    }
}