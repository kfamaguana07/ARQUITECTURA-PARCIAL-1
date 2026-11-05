using System;
using System.Data.SqlClient;
using WS_Comercializadora_Dotnet_Soap_GR01.Models;

namespace WS_Comercializadora_Dotnet_Soap_GR01.DataAccess
{
    public class ClienteDAL
    {
        public ClienteComercializadora ObtenerPorCedula(string cedula)
        {
            ClienteComercializadora cliente = null;
            using (var conn = DbConnection.GetConnection())
            {
                string query = @"SELECT CLI_ID, CLI_CEDULA, CLI_NOMBRE, CLI_APELLIDO, CLI_TELEFONO, CLI_DIRECCION 
                                FROM CLIENTE WHERE CLI_CEDULA = @Cedula";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Cedula", cedula);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        cliente = new ClienteComercializadora
                        {
                            CliId = Convert.ToInt32(reader["CLI_ID"]),
                            CliCedula = reader["CLI_CEDULA"].ToString(),
                            CliNombre = reader["CLI_NOMBRE"].ToString(),
                            CliApellido = reader["CLI_APELLIDO"].ToString(),
                            CliTelefono = reader["CLI_TELEFONO"] != DBNull.Value ? reader["CLI_TELEFONO"].ToString() : "",
                            CliDireccion = reader["CLI_DIRECCION"] != DBNull.Value ? reader["CLI_DIRECCION"].ToString() : ""
                        };
                    }
                }
            }
            return cliente;
        }
    }
}