using System;
using System.Collections.Generic;
using WS_Banquito_Dotnet_Soap_GR01.BusinessLogic;
using WS_Banquito_Dotnet_Soap_GR01.DataAccess;
using WS_Banquito_Dotnet_Soap_GR01.Models;

namespace WS_Banquito_Dotnet_Soap_GR01.Services
{
    public class BanquitoService : IBanquitoService
    {
        private readonly ValidacionCreditoBLL _validacionBLL = new ValidacionCreditoBLL();
        private readonly CapacidadPagoBLL _capacidadPagoBLL = new CapacidadPagoBLL();
        private readonly AmortizacionBLL _amortizacionBLL = new AmortizacionBLL();
        private readonly CreditoDAL _creditoDAL = new CreditoDAL();

        /// <summary>
        /// Valida las 4 reglas del negocio para saber si el cliente es sujeto de crédito.
        /// </summary>
        public RespuestaValidacion ValidarSujetoCredito(string cedula)
        {
            if (string.IsNullOrWhiteSpace(cedula))
            {
                return new RespuestaValidacion
                {
                    EsSujeto = false,
                    CodigoCliente = 0,
                    Mensaje = "Debe ingresar un número de cédula válido."
                };
            }

            return _validacionBLL.Validar(cedula.Trim());
        }

        /// <summary>
        /// Retorna el cupo máximo de crédito calculado para un cliente.
        /// </summary>
        public decimal ObtenerMontoMaximo(int codigoCliente)
        {
            if (codigoCliente <= 0) return 0;
            return _capacidadPagoBLL.CalcularMontoMaximo(codigoCliente);
        }

        /// <summary>
        /// Genera la tabla de amortización y guarda el crédito en la BD del banco.
        /// </summary>
        public int OtorgarCredito(int codigoCliente, decimal precioTelefono, int numeroCuotas)
        {
            try
            {
                if (codigoCliente <= 0 || precioTelefono <= 0 || numeroCuotas < 3 || numeroCuotas > 18)
                {
                    return 0;
                }

                // 1. Generar la lista de cuotas en memoria según el sistema francés
                List<CuotaAmortizacion> tabla = _amortizacionBLL.GenerarTabla(precioTelefono, numeroCuotas);

                // 2. Construir la cabecera del crédito
                Credito credito = new Credito
                {
                    CodCliente = codigoCliente,
                    Monto = precioTelefono,
                    PlazoMeses = numeroCuotas,
                    TasaInteres = 16.50m,
                    FechaCreacion = DateTime.Now,
                    TablaAmortizacion = tabla
                };

                // 3. Persistir en la BD dentro de una transacción SQL
                return _creditoDAL.GuardarCreditoConAmortizacion(credito);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// Consulta la tabla de amortización almacenada para un crédito.
        /// </summary>
        public List<CuotaAmortizacion> ConsultarTablaAmortizacion(int codigoCredito)
        {
            if (codigoCredito <= 0) return new List<CuotaAmortizacion>();
            return _creditoDAL.ObtenerTablaAmortizacion(codigoCredito);
        }
    }
}