using System;
using System.Data.SqlClient;
using WS_Comercializadora_Dotnet_Soap_GR01.Models;

namespace WS_Comercializadora_Dotnet_Soap_GR01.DataAccess
{
    public class FacturaDAL
    {
        public int GuardarFactura(Factura factura, int productoId)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string insertFactura = @"
                            INSERT INTO FACTURA (CLI_ID, FAC_FECHA, FAC_FORMAPAGO, FAC_PRECIOTOTAL, FAC_NUMCUOTAS, FAC_PRECIOCUOTA)
                            VALUES (@CliId, GETDATE(), @FormaPago, @PrecioTotal, @NumCuotas, @PrecioCuota);
                            SELECT SCOPE_IDENTITY();";

                        SqlCommand cmdFactura = new SqlCommand(insertFactura, conn, transaction);
                        cmdFactura.Parameters.AddWithValue("@CliId", factura.CliId);
                        cmdFactura.Parameters.AddWithValue("@FormaPago", factura.FacFormaPago);
                        cmdFactura.Parameters.AddWithValue("@PrecioTotal", factura.FacPrecioTotal);
                        cmdFactura.Parameters.AddWithValue("@NumCuotas", (object)factura.FacNumCuotas ?? DBNull.Value);
                        cmdFactura.Parameters.AddWithValue("@PrecioCuota", (object)factura.FacPrecioCuota ?? DBNull.Value);

                        int facId = Convert.ToInt32(cmdFactura.ExecuteScalar());

                        string insertPosee = @"
                            INSERT INTO POSEE (PRO_ID, FAC_ID)
                            VALUES (@ProId, @FacId)";

                        SqlCommand cmdPosee = new SqlCommand(insertPosee, conn, transaction);
                        cmdPosee.Parameters.AddWithValue("@ProId", productoId);
                        cmdPosee.Parameters.AddWithValue("@FacId", facId);
                        cmdPosee.ExecuteNonQuery();

                        transaction.Commit();
                        return facId;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}