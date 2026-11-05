using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using WS_Banquito_Dotnet_Soap_GR01.Models;

namespace WS_Banquito_Dotnet_Soap_GR01.DataAccess
{
    public class CreditoDAL
    {
        public int GuardarCreditoConAmortizacion(Credito credito)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string insertCreditoQuery = @"
                            INSERT INTO CREDITO (COD_CLIENTE, MONTO, PLAZO_MESES, TASA_INTERES, FECHA_CREACION)
                            VALUES (@CodCliente, @Monto, @PlazoMeses, @TasaInteres, GETDATE());
                            SELECT SCOPE_IDENTITY();";

                        SqlCommand cmdCredito = new SqlCommand(insertCreditoQuery, conn, transaction);
                        cmdCredito.Parameters.AddWithValue("@CodCliente", credito.CodCliente);
                        cmdCredito.Parameters.AddWithValue("@Monto", credito.Monto);
                        cmdCredito.Parameters.AddWithValue("@PlazoMeses", credito.PlazoMeses);
                        cmdCredito.Parameters.AddWithValue("@TasaInteres", credito.TasaInteres);

                        int codCredito = Convert.ToInt32(cmdCredito.ExecuteScalar());

                        string insertAmortizacionQuery = @"
                            INSERT INTO AMORTIZACION (COD_CREDITO, NUM_CUOTA, VALOR_CUOTA, INTERES_PAGADO, CAPITAL_PAGADO, SALDO_RESTANTE)
                            VALUES (@CodCredito, @NumCuota, @ValorCuota, @InteresPagado, @CapitalPagado, @SaldoRestante)";

                        foreach (var cuota in credito.TablaAmortizacion)
                        {
                            SqlCommand cmdCuota = new SqlCommand(insertAmortizacionQuery, conn, transaction);
                            cmdCuota.Parameters.AddWithValue("@CodCredito", codCredito);
                            cmdCuota.Parameters.AddWithValue("@NumCuota", cuota.NumCuota);
                            cmdCuota.Parameters.AddWithValue("@ValorCuota", cuota.ValorCuota);
                            cmdCuota.Parameters.AddWithValue("@InteresPagado", cuota.InteresPagado);
                            cmdCuota.Parameters.AddWithValue("@CapitalPagado", cuota.CapitalPagado);
                            cmdCuota.Parameters.AddWithValue("@SaldoRestante", cuota.SaldoRestante);
                            cmdCuota.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        return codCredito;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public List<CuotaAmortizacion> ObtenerTablaAmortizacion(int codCredito)
        {
            var lista = new List<CuotaAmortizacion>();

            using (var conn = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT NUM_CUOTA, VALOR_CUOTA, INTERES_PAGADO, CAPITAL_PAGADO, SALDO_RESTANTE
                    FROM AMORTIZACION
                    WHERE COD_CREDITO = @CodCredito
                    ORDER BY NUM_CUOTA ASC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CodCredito", codCredito);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new CuotaAmortizacion
                        {
                            NumCuota = Convert.ToInt32(reader["NUM_CUOTA"]),
                            ValorCuota = Convert.ToDecimal(reader["VALOR_CUOTA"]),
                            InteresPagado = Convert.ToDecimal(reader["INTERES_PAGADO"]),
                            CapitalPagado = Convert.ToDecimal(reader["CAPITAL_PAGADO"]),
                            SaldoRestante = Convert.ToDecimal(reader["SALDO_RESTANTE"])
                        });
                    }
                }
            }

            return lista;
        }
    }
}