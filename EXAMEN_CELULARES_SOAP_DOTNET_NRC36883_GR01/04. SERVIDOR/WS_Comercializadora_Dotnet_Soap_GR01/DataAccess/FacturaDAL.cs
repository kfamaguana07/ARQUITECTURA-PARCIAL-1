using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using WS_Comercializadora_Dotnet_Soap_GR01.Models;

namespace WS_Comercializadora_Dotnet_Soap_GR01.DataAccess
{
    public class FacturaDetalleItem
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal => PrecioUnitario * Cantidad;
    }

    public class FacturaDAL
    {
        public int GuardarFactura(Factura factura, List<FacturaDetalleItem> detalles)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // 1. Insertar Cabecera en FACTURA
                        string insertFactura = @"
                            INSERT INTO FACTURA (CLI_ID, FAC_FECHA, FAC_FORMAPAGO, FAC_SUBTOTAL, FAC_DESCUENTO, FAC_PRECIOTOTAL, FAC_NUMCUOTAS, FAC_PRECIOCUOTA, CRE_ID)
                            VALUES (@CliId, GETDATE(), @FormaPago, @Subtotal, @Descuento, @PrecioTotal, @NumCuotas, @PrecioCuota, @CreId);
                            SELECT SCOPE_IDENTITY();";

                        SqlCommand cmdFactura = new SqlCommand(insertFactura, conn, transaction);
                        cmdFactura.Parameters.AddWithValue("@CliId", factura.CliId);
                        cmdFactura.Parameters.AddWithValue("@FormaPago", factura.FacFormaPago);
                        cmdFactura.Parameters.AddWithValue("@Subtotal", factura.FacSubtotal);
                        cmdFactura.Parameters.AddWithValue("@Descuento", factura.FacDescuento);
                        cmdFactura.Parameters.AddWithValue("@PrecioTotal", factura.FacPrecioTotal);
                        cmdFactura.Parameters.AddWithValue("@NumCuotas", (object)factura.FacNumCuotas ?? DBNull.Value);
                        cmdFactura.Parameters.AddWithValue("@PrecioCuota", (object)factura.FacPrecioCuota ?? DBNull.Value);
                        cmdFactura.Parameters.AddWithValue("@CreId", (object)factura.CreId ?? DBNull.Value);

                        int facId = Convert.ToInt32(cmdFactura.ExecuteScalar());

                        // 2. Insertar Detalle en POSEE
                        string insertPosee = @"
                            INSERT INTO POSEE (PRO_ID, FAC_ID, POS_CANTIDAD, POS_PRECIO_UNITARIO, POS_SUBTOTAL)
                            VALUES (@ProId, @FacId, @Cantidad, @PrecioUnitario, @Subtotal)";

                        foreach (var item in detalles)
                        {
                            SqlCommand cmdPosee = new SqlCommand(insertPosee, conn, transaction);
                            cmdPosee.Parameters.AddWithValue("@ProId", item.ProductoId);
                            cmdPosee.Parameters.AddWithValue("@FacId", facId);
                            cmdPosee.Parameters.AddWithValue("@Cantidad", item.Cantidad);
                            cmdPosee.Parameters.AddWithValue("@PrecioUnitario", item.PrecioUnitario);
                            cmdPosee.Parameters.AddWithValue("@Subtotal", item.Subtotal);
                            cmdPosee.ExecuteNonQuery();
                        }

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

        public List<FacturaConsultaContract> ObtenerHistorialFacturas(string cedulaCliente)
        {
            var historial = new List<FacturaConsultaContract>();

            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();

                // Construcción limpia de la consulta según si se pasa cédula o se solicitan todas
                bool tieneCedula = !string.IsNullOrWhiteSpace(cedulaCliente);

                string queryFacturas = @"
            SELECT 
                f.FAC_ID,
                c.CLI_CEDULA,
                f.FAC_FECHA,
                f.FAC_FORMAPAGO,
                f.FAC_SUBTOTAL,
                f.FAC_DESCUENTO,
                f.FAC_PRECIOTOTAL,
                f.CRE_ID
            FROM FACTURA f
            INNER JOIN CLIENTE c ON f.CLI_ID = c.CLI_ID " +
                    (tieneCedula ? "WHERE c.CLI_CEDULA = @Cedula " : "") +
                    "ORDER BY f.FAC_FECHA DESC;";

                using (var cmdFact = new SqlCommand(queryFacturas, conn))
                {
                    if (tieneCedula)
                    {
                        cmdFact.Parameters.AddWithValue("@Cedula", cedulaCliente.Trim());
                    }

                    using (var readerFact = cmdFact.ExecuteReader())
                    {
                        while (readerFact.Read())
                        {
                            var fact = new FacturaConsultaContract
                            {
                                FacturaId = Convert.ToInt32(readerFact["FAC_ID"]),
                                CedulaCliente = readerFact["CLI_CEDULA"] != DBNull.Value ? readerFact["CLI_CEDULA"].ToString() : string.Empty,
                                FechaVenta = readerFact["FAC_FECHA"] != DBNull.Value ? Convert.ToDateTime(readerFact["FAC_FECHA"]) : DateTime.Now,
                                FormaPago = readerFact["FAC_FORMAPAGO"] != DBNull.Value ? readerFact["FAC_FORMAPAGO"].ToString() : "E",
                                Subtotal = readerFact["FAC_SUBTOTAL"] != DBNull.Value ? Convert.ToDecimal(readerFact["FAC_SUBTOTAL"]) : 0m,
                                Descuento = readerFact["FAC_DESCUENTO"] != DBNull.Value ? Convert.ToDecimal(readerFact["FAC_DESCUENTO"]) : 0m,
                                Total = readerFact["FAC_PRECIOTOTAL"] != DBNull.Value ? Convert.ToDecimal(readerFact["FAC_PRECIOTOTAL"]) : 0m,
                                CreditoId = readerFact["CRE_ID"] != DBNull.Value ? (int?)Convert.ToInt32(readerFact["CRE_ID"]) : null,
                                Detalle = new List<DetalleFacturaConsultaContract>()
                            };

                            historial.Add(fact);
                        }
                    }
                }

                // Cargar detalles solo si se encontraron facturas
                foreach (var factura in historial)
                {
                    string queryDetalle = @"
                SELECT 
                    p.PRO_ID,
                    p.PRO_NOMBRE,
                    pos.POS_CANTIDAD,
                    pos.POS_PRECIO_UNITARIO
                FROM POSEE pos
                INNER JOIN PRODUCTO p ON pos.PRO_ID = p.PRO_ID
                WHERE pos.FAC_ID = @FacturaId;";

                    using (var cmdDet = new SqlCommand(queryDetalle, conn))
                    {
                        cmdDet.Parameters.AddWithValue("@FacturaId", factura.FacturaId);

                        using (var readerDet = cmdDet.ExecuteReader())
                        {
                            while (readerDet.Read())
                            {
                                int cant = Convert.ToInt32(readerDet["POS_CANTIDAD"]);
                                decimal prec = Convert.ToDecimal(readerDet["POS_PRECIO_UNITARIO"]);

                                factura.Detalle.Add(new DetalleFacturaConsultaContract
                                {
                                    ProductoId = Convert.ToInt32(readerDet["PRO_ID"]),
                                    ProductoNombre = readerDet["PRO_NOMBRE"] != DBNull.Value ? readerDet["PRO_NOMBRE"].ToString() : string.Empty,
                                    Cantidad = cant,
                                    PrecioUnitario = prec,
                                    Subtotal = cant * prec
                                });
                            }
                        }
                    }
                }
            }

            return historial;
        }
    }
}