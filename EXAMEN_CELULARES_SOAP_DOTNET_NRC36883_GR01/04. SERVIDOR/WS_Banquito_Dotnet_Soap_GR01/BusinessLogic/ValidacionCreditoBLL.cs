using System;
using WS_Banquito_Dotnet_Soap_GR01.DataAccess;
using WS_Banquito_Dotnet_Soap_GR01.Models;

namespace WS_Banquito_Dotnet_Soap_GR01.BusinessLogic
{
    public class ValidacionCreditoBLL
    {
        private readonly ClienteDAL _clienteDAL = new ClienteDAL();
        private readonly MovimientoDAL _movimientoDAL = new MovimientoDAL();

        public RespuestaValidacion Validar(string cedula)
        {
            // Regla 1: Verificar si el solicitante es cliente del banco
            var cliente = _clienteDAL.ObtenerClientePorCedula(cedula);
            if (cliente == null)
            {
                return new RespuestaValidacion
                {
                    EsSujeto = false,
                    CodigoCliente = 0,
                    Mensaje = "El solicitante no es cliente del banco."
                };
            }

            // Regla 2: Verificar al menos un depósito en el último mes
            bool tieneDeposito = _movimientoDAL.TieneDepositoUltimoMes(cliente.CodCliente);
            if (!tieneDeposito)
            {
                return new RespuestaValidacion
                {
                    EsSujeto = false,
                    CodigoCliente = cliente.CodCliente,
                    Mensaje = "El cliente no posee movimientos de depósito en el último mes."
                };
            }

            // Regla 3: Si es masculino, verificar que no sea menor de 25 años
            if (cliente.Genero.Equals("M", StringComparison.OrdinalIgnoreCase))
            {
                int edad = DateTime.Today.Year - cliente.FechaNacimiento.Year;
                if (cliente.FechaNacimiento.Date > DateTime.Today.AddYears(-edad)) edad--;

                if (edad < 25)
                {
                    return new RespuestaValidacion
                    {
                        EsSujeto = false,
                        CodigoCliente = cliente.CodCliente,
                        Mensaje = "El cliente es menor de 25 años."
                    };
                }
            }

            // Regla 4: Verificar que no tenga un crédito activo
            bool tieneCredito = _clienteDAL.TieneCreditoActivo(cliente.CodCliente);
            if (tieneCredito)
            {
                return new RespuestaValidacion
                {
                    EsSujeto = false,
                    CodigoCliente = cliente.CodCliente,
                    Mensaje = "El cliente actualmente ya posee un crédito activo en el banco."
                };
            }

            return new RespuestaValidacion
            {
                EsSujeto = true,
                CodigoCliente = cliente.CodCliente,
                Mensaje = "Aprobado: El cliente es sujeto de crédito."
            };
        }
    }
}