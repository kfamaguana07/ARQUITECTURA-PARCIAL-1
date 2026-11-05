using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WS_Eurekabank_Dotnet_Rest_GR01.DTOs;
using WS_Eurekabank_Dotnet_Rest_GR01.Models;

namespace WS_Eurekabank_Dotnet_Rest_GR01.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Ruta base: /api/movimientos
    public class MovimientosController : ControllerBase
    {
        private readonly EurekaBankContext _context;
        private const string DEFAULT_COD_EMPLEADO = "0001";

        // Constantes de tipos de movimiento
        private const string TIPO_DEPOSITO = "003";
        private const string TIPO_RETIRO = "004";
        private const string TIPO_TRANSF_INGRESO = "008";
        private const string TIPO_TRANSF_SALIDA = "009";

        public MovimientosController(EurekaBankContext context)
        {
            _context = context;
        }

        // ==============================================================
        // 1. OBTENER TODAS LAS CUENTAS
        // GET: api/movimientos/cuentas
        // ==============================================================
        [HttpGet("cuentas")]
        public async Task<IActionResult> ObtenerCuentas()
        {
            try
            {
                var cuentas = await (from c in _context.Cuentas
                                     join cl in _context.Clientes on c.ChrClieCodigo equals cl.ChrClieCodigo
                                     where c.VchCuenEstado == "ACTIVO"
                                     orderby c.ChrCuenCodigo
                                     select new DatosCuentaDTO
                                     {
                                         Codigo = c.ChrCuenCodigo,
                                         Moneda = c.ChrMoneCodigo,
                                         Saldo = c.DecCuenSaldo,
                                         Estado = c.VchCuenEstado,
                                         NombreCliente = cl.VchCliePaterno + " " + cl.VchClieMaterno + ", " + cl.VchClieNombre,
                                         EmailCliente = cl.VchClieEmail,
                                         TelefonoCliente = cl.VchClieTelefono
                                     }).ToListAsync();

                return Ok(cuentas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResultadoOperacionDTO.Error("Error al obtener cuentas: " + ex.Message));
            }
        }

        // ==============================================================
        // 2. OBTENER MOVIMIENTOS (Filtro opcional por cuenta)
        // GET: api/movimientos?cuenta=00100001
        // ==============================================================
        [HttpGet]
        public async Task<IActionResult> ObtenerMovimientos([FromQuery] string? cuenta)
        {
            try
            {
                var query = from m in _context.Movimientos
                            join c in _context.Cuentas on m.ChrCuenCodigo equals c.ChrCuenCodigo
                            join cl in _context.Clientes on c.ChrClieCodigo equals cl.ChrClieCodigo
                            join tm in _context.TiposMovimiento on m.ChrTipoCodigo equals tm.ChrTipoCodigo
                            select new MovimientoDataDTO
                            {
                                CodigoCuenta = m.ChrCuenCodigo,
                                Numero = m.IntMoviNumero,
                                Fecha = m.DttMoviFecha,
                                Tipo = tm.VchTipoDescripcion + (tm.VchTipoAccion == "INGRESO" ? " Entrada" : " Salida"),
                                Importe = m.DecMoviImporte,
                                Referencia = m.ChrCuenReferencia ?? "",
                                NombreCliente = (cl.VchCliePaterno ?? "") + " " + (cl.VchClieMaterno ?? "") + ", " + (cl.VchClieNombre ?? "")
                            };

                if (!string.IsNullOrEmpty(cuenta))
                {
                    query = query.Where(x => x.CodigoCuenta == cuenta);
                }

                var listaCruda = await query
                    .OrderBy(x => x.Fecha)
                    .ThenBy(x => x.Numero)
                    .ToListAsync();

                decimal saldoAcumulado = 0;
                foreach (var mov in listaCruda)
                {
                    bool esIngreso = mov.Tipo.Contains("Entrada");
                    decimal importeReal = esIngreso ? mov.Importe : -mov.Importe;
                    saldoAcumulado += importeReal;
                    mov.SaldoActual = saldoAcumulado;
                }

                var listaFinal = listaCruda.OrderByDescending(x => x.Fecha).ThenByDescending(x => x.Numero).ToList();
                return Ok(listaFinal);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResultadoOperacionDTO.Error("Error al obtener movimientos: " + ex.Message));
            }
        }


        // ==============================================================
        // 3. REGISTRAR MOVIMIENTO (Depósito, Retiro, Transferencia)
        // POST: api/movimientos
        // ==============================================================
        [HttpPost]
        public async Task<IActionResult> RegistrarMovimiento([FromBody] MovimientoRequestDTO request)
        {
            if (request == null || string.IsNullOrEmpty(request.CuentaOrigen) || request.Importe <= 0)
            {
                return BadRequest(ResultadoOperacionDTO.Error("Datos de movimiento inválidos. Verifique la cuenta y el importe."));
            }

            // Iniciamos una transacción para asegurar consistencia
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                string tipoUpper = request.Tipo?.ToUpper() ?? "";

                switch (tipoUpper)
                {
                    case "DEPOSITO":
                        await ProcesarCuenta(request.CuentaOrigen, request.Importe, DEFAULT_COD_EMPLEADO, TIPO_DEPOSITO, true, null);
                        break;

                    case "RETIRO":
                        await ProcesarCuenta(request.CuentaOrigen, request.Importe, DEFAULT_COD_EMPLEADO, TIPO_RETIRO, false, null);
                        break;

                    case "TRANSFERENCIA":
                        if (string.IsNullOrEmpty(request.CuentaDestino))
                            return BadRequest(ResultadoOperacionDTO.Error("Cuenta destino es requerida para transferencia."));
                        if (request.CuentaOrigen == request.CuentaDestino)
                            return BadRequest(ResultadoOperacionDTO.Error("No se puede transferir a la misma cuenta."));

                        // Procesar salida de origen
                        await ProcesarCuenta(request.CuentaOrigen, request.Importe, DEFAULT_COD_EMPLEADO, TIPO_TRANSF_SALIDA, false, request.CuentaDestino);
                        // Procesar ingreso a destino
                        await ProcesarCuenta(request.CuentaDestino, request.Importe, DEFAULT_COD_EMPLEADO, TIPO_TRANSF_INGRESO, true, request.CuentaOrigen);
                        break;

                    default:
                        return BadRequest(ResultadoOperacionDTO.Error("Tipo de movimiento no válido."));
                }

                // Guardamos los cambios en la base de datos
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(ResultadoOperacionDTO.Exito($"{(tipoUpper == "TRANSFERENCIA" ? "Transferencia" : tipoUpper)} realizado con éxito."));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, ResultadoOperacionDTO.Error("Error al procesar el movimiento: " + ex.Message));
            }
        }

        // ==============================================================
        // MÉTODO AUXILIAR: Procesar una cuenta (reutilizable)
        // ==============================================================
        private async Task ProcesarCuenta(string codigoCuenta, decimal importe, string codEmpleado, string tipoCodigo, bool esIngreso, string? cuentaReferencia)
        {
            var cuenta = await _context.Cuentas.FirstOrDefaultAsync(c => c.ChrCuenCodigo == codigoCuenta);

            if (cuenta == null)
                throw new Exception($"La cuenta {codigoCuenta} no existe.");

            if (cuenta.VchCuenEstado != "ACTIVO")
                throw new Exception($"La cuenta {codigoCuenta} no está activa.");

            if (!esIngreso && cuenta.DecCuenSaldo < importe)
                throw new Exception($"Saldo insuficiente en la cuenta {codigoCuenta}.");

            // 1. Actualizar el saldo
            cuenta.DecCuenSaldo += esIngreso ? importe : -importe;

            // 2. Incrementar el contador de movimientos
            cuenta.IntCuenContMov++;
            int nuevoNumeroMovimiento = cuenta.IntCuenContMov;

            // 3. Crear el registro del movimiento
            var nuevoMovimiento = new Movimiento
            {
                ChrCuenCodigo = codigoCuenta,
                IntMoviNumero = nuevoNumeroMovimiento,
                DttMoviFecha = DateTime.Now,
                ChrEmplCodigo = codEmpleado,
                ChrTipoCodigo = tipoCodigo,
                DecMoviImporte = importe,
                ChrCuenReferencia = cuentaReferencia
            };

            _context.Movimientos.Add(nuevoMovimiento);
        }
    }
}