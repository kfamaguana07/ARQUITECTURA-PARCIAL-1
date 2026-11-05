using System;
using System.Data.SqlClient;
using WS_Banquito_Dotnet_Soap_GR01.Models;

namespace WS_Banquito_Dotnet_Soap_GR01.DataAccess
{
    public class ClienteDAL
    {
        public Cliente ObtenerClientePorCedula(string cedula)
        {
            Cliente cliente = null;
            using (var conn = DbConnection.GetConnection())
            {
                string query = "SELECT COD_CLIENTE, CEDULA, NOMBRE, GENERO, FECHA_NACIMIENTO FROM CLIENTE WHERE CEDULA = @Cedula";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Cedula", cedula);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        cliente = new Cliente
                        {
                            CodCliente = Convert.ToInt32(reader["COD_CLIENTE"]),
                            Cedula = reader["CEDULA"].ToString(),
                            Nombre = reader["NOMBRE"].ToString(),
                            Genero = reader["GENERO"].ToString(),
                            FechaNacimiento = Convert.ToDateTime(reader["FECHA_NACIMIENTO"])
                        };
                    }
                }
            }
            return cliente;
        }

        public bool TieneCreditoActivo(int codCliente)
        {
            using (var conn = DbConnection.GetConnection())
            {
                string query = "SELECT COUNT(1) FROM CREDITO WHERE COD_CLIENTE = @CodCliente";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CodCliente", codCliente);
                conn.Open();

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }
    }
}