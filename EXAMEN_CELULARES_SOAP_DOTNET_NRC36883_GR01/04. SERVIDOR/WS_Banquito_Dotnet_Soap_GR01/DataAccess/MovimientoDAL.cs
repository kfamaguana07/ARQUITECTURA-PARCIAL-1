using System;
using System.Data.SqlClient;

namespace WS_Banquito_Dotnet_Soap_GR01.DataAccess
{
    public class MovimientoDAL
    {
        public bool TieneDepositoUltimoMes(int codCliente)
        {
            using (var conn = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT COUNT(1) 
                    FROM MOVIMIENTO m
                    INNER JOIN CUENTA c ON m.NUM_CUENTA = c.NUM_CUENTA
                    WHERE c.COD_CLIENTE = @CodCliente 
                      AND m.TIPO = 'DEP'
                      AND m.FECHA >= DATEADD(month, -1, GETDATE())";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CodCliente", codCliente);
                conn.Open();

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }

        public void ObtenerPromediosUltimos3Meses(int codCliente, out decimal promedioDepositos, out decimal promedioRetiros)
        {
            promedioDepositos = 0;
            promedioRetiros = 0;

            using (var conn = DbConnection.GetConnection())
            {
                string query = @"
                    SELECT 
                        ISNULL(SUM(CASE WHEN m.TIPO = 'DEP' THEN m.VALOR ELSE 0 END) / 3.0, 0) AS PromedioDepositos,
                        ISNULL(SUM(CASE WHEN m.TIPO = 'RET' THEN m.VALOR ELSE 0 END) / 3.0, 0) AS PromedioRetiros
                    FROM MOVIMIENTO m
                    INNER JOIN CUENTA c ON m.NUM_CUENTA = c.NUM_CUENTA
                    WHERE c.COD_CLIENTE = @CodCliente
                      AND m.FECHA >= DATEADD(month, -3, GETDATE())";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CodCliente", codCliente);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        promedioDepositos = Convert.ToDecimal(reader["PromedioDepositos"]);
                        promedioRetiros = Convert.ToDecimal(reader["PromedioRetiros"]);
                    }
                }
            }
        }
    }
}