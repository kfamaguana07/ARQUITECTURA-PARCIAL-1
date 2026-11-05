using System;
using System.Collections.Generic;
using System.Linq;
using WS_Eurekabank_Dotnet_Soap_GR01.Interfaces;
using WS_Eurekabank_Dotnet_Soap_GR01.Models;

namespace WS_Eurekabank_Dotnet_Soap_GR01.Services
{
    public class MovimientosService : IMovimientosService
    {
        private const string EmpleadoDefault = "0001";
        private const string TipoDeposito = "003";
        private const string TipoRetiro = "004";
        private const string TipoTransferenciaSalida = "009";
        private const string TipoTransferenciaIngreso = "008";
        public List<MovimientoData> GetMovimientos(string cuenta)
        {
            using (var context = new EurekaBankContext())
            {
                var query = from m in context.Movimientos
                            join c in context.Cuentas on m.ChrCuenCodigo equals c.ChrCuenCodigo
                            join cl in context.Clientes on c.ChrClieCodigo equals cl.ChrClieCodigo
                            join tm in context.TiposMovimiento on m.ChrTipoCodigo equals tm.ChrTipoCodigo
                            select new { m, c, cl, tm };

                if (!string.IsNullOrWhiteSpace(cuenta) && cuenta != "?")
                {
                    query = query.Where(x => x.m.ChrCuenCodigo == cuenta);
                }

                var movimientosConExtras = query.OrderBy(x => x.m.DttMoviFecha).ThenBy(x => x.m.IntMoviNumero)
                    .Select(x => new
                    {
                        Data = new MovimientoData
                        {
                            Numero = x.m.IntMoviNumero,
                            Fecha = x.m.DttMoviFecha,
                            Tipo = x.tm.VchTipoDescripcion + (x.tm.ChrTipoCodigo == "008" ? " Entrada" : x.tm.ChrTipoCodigo == "009" ? " Salida" : ""),
                            Importe = x.m.DecMoviImporte * (x.tm.VchTipoAccion == "INGRESO" ? 1 : -1),
                            Referencia = x.m.ChrCuenReferencia,
                            CodigoCuenta = x.m.ChrCuenCodigo,
                            NombreCliente = x.cl.VchClieNombre + " " + x.cl.VchCliePaterno + " " + x.cl.VchClieMaterno
                        },
                        TipoAccion = x.tm.VchTipoAccion,
                        SaldoCuenta = x.c.DecCuenSaldo
                    }).ToList();

                // Calcular SaldoActual por grupo de cuenta
                var grupos = movimientosConExtras.GroupBy(me => me.Data.CodigoCuenta);
                foreach (var grupo in grupos)
                {
                    var lista = grupo.OrderBy(me => me.Data.Fecha).ThenBy(me => me.Data.Numero).ToList();  // Ascending order
                    decimal saldoActual = lista.Last().SaldoCuenta - lista.Sum(me => me.Data.Importe);  // Initial saldo

                    foreach (var me in lista)
                    {
                        saldoActual += me.Data.Importe;
                        me.Data.SaldoActual = saldoActual;
                    }
                }

                return movimientosConExtras.Select(me => me.Data).OrderByDescending(md => md.Fecha).ThenByDescending(md => md.Numero).ToList();
            }
        }

        public ResultadoOperacion ProcesarMovimiento(string tipo, string cuenta, string cuentaDestino, decimal monto)
        {
            if (monto <= 0) return new ResultadoOperacion(-1, "El monto debe ser mayor a cero.");

            switch (tipo.ToUpper())
            {
                case "DEPOSITO":
                    return Deposito(cuenta, monto) ? new ResultadoOperacion(1, "Depósito realizado exitosamente.") : new ResultadoOperacion(-1, "Error al realizar el depósito.");
                case "RETIRO":
                    return Retiro(cuenta, monto) ? new ResultadoOperacion(1, "Retiro realizado exitosamente.") : new ResultadoOperacion(-1, "Error al realizar el retiro. Verifique saldo suficiente.");
                case "TRANSFERENCIA":
                    if (string.IsNullOrEmpty(cuentaDestino)) return new ResultadoOperacion(-1, "Cuenta destino requerida para transferencia.");
                    return Transferencia(cuenta, cuentaDestino, monto) ? new ResultadoOperacion(1, "Transferencia realizada exitosamente.") : new ResultadoOperacion(-1, "Error al realizar la transferencia. Verifique cuentas y saldo.");
                default:
                    return new ResultadoOperacion(-1, "Tipo de movimiento no válido.");
            }
        }

        private bool Deposito(string cuenta, decimal monto)
        {
            using (var context = new EurekaBankContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var c = context.Cuentas.FirstOrDefault(cu => cu.ChrCuenCodigo == cuenta);
                        if (c == null) return false;
                        int nextNum = c.IntCuenContMov + 1;
                        c.DecCuenSaldo += monto;
                        c.IntCuenContMov = nextNum;
                        var mov = new Movimiento
                        {
                            ChrCuenCodigo = cuenta,
                            IntMoviNumero = nextNum,
                            DttMoviFecha = DateTime.Now.Date,
                            ChrEmplCodigo = EmpleadoDefault,
                            ChrTipoCodigo = TipoDeposito,
                            DecMoviImporte = monto
                        };
                        context.Movimientos.Add(mov);
                        context.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        private bool Retiro(string cuenta, decimal monto)
        {
            using (var context = new EurekaBankContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var c = context.Cuentas.FirstOrDefault(cu => cu.ChrCuenCodigo == cuenta);
                        if (c == null || c.DecCuenSaldo < monto) return false;
                        int nextNum = c.IntCuenContMov + 1;
                        c.DecCuenSaldo -= monto;
                        c.IntCuenContMov = nextNum;
                        var mov = new Movimiento
                        {
                            ChrCuenCodigo = cuenta,
                            IntMoviNumero = nextNum,
                            DttMoviFecha = DateTime.Now.Date,
                            ChrEmplCodigo = EmpleadoDefault,
                            ChrTipoCodigo = TipoRetiro,
                            DecMoviImporte = monto
                        };
                        context.Movimientos.Add(mov);
                        context.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        private bool Transferencia(string cuentaOrigen, string cuentaDestino, decimal monto)
        {
            if (cuentaOrigen == cuentaDestino) return false;
            using (var context = new EurekaBankContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var origen = context.Cuentas.FirstOrDefault(cu => cu.ChrCuenCodigo == cuentaOrigen);
                        var destino = context.Cuentas.FirstOrDefault(cu => cu.ChrCuenCodigo == cuentaDestino);
                        if (origen == null || destino == null || origen.DecCuenSaldo < monto || destino.VchCuenEstado != "ACTIVO") return false;

                        int nextNumOri = origen.IntCuenContMov + 1;
                        int nextNumDest = destino.IntCuenContMov + 1;

                        origen.DecCuenSaldo -= monto;
                        origen.IntCuenContMov = nextNumOri;
                        destino.DecCuenSaldo += monto;
                        destino.IntCuenContMov = nextNumDest;

                        var movOri = new Movimiento
                        {
                            ChrCuenCodigo = cuentaOrigen,
                            IntMoviNumero = nextNumOri,
                            DttMoviFecha = DateTime.Now.Date,
                            ChrEmplCodigo = EmpleadoDefault,
                            ChrTipoCodigo = TipoTransferenciaSalida,
                            DecMoviImporte = monto,
                            ChrCuenReferencia = cuentaDestino
                        };
                        var movDest = new Movimiento
                        {
                            ChrCuenCodigo = cuentaDestino,
                            IntMoviNumero = nextNumDest,
                            DttMoviFecha = DateTime.Now.Date,
                            ChrEmplCodigo = EmpleadoDefault,
                            ChrTipoCodigo = TipoTransferenciaIngreso,
                            DecMoviImporte = monto,
                            ChrCuenReferencia = cuentaOrigen
                        };
                        context.Movimientos.Add(movOri);
                        context.Movimientos.Add(movDest);
                        context.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        public List<CuentaData> GetDatosCuentas()
        {
            using (var context = new EurekaBankContext())
            {
                var cuentasData = (from c in context.Cuentas
                                   join cl in context.Clientes on c.ChrClieCodigo equals cl.ChrClieCodigo
                                   select new CuentaData
                                   {
                                       Codigo = c.ChrCuenCodigo,
                                       Moneda = c.ChrMoneCodigo,
                                       Saldo = c.DecCuenSaldo,
                                       Estado = c.VchCuenEstado,
                                       NombreCliente = cl.VchClieNombre + " " + cl.VchCliePaterno + " " + cl.VchClieMaterno,
                                       EmailCliente = cl.VchClieEmail,
                                       TelefonoCliente = cl.VchClieTelefono
                                   }).ToList();
                return cuentasData;
            }
        }

        public List<string> GetTiposMovimiento()
        {
            return new List<string> { "DEPOSITO", "RETIRO", "TRANSFERENCIA" };
        }
    }
}
