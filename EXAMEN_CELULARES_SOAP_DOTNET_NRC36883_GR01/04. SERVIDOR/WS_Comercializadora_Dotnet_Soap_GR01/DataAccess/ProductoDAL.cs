using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using WS_Comercializadora_Dotnet_Soap_GR01.Models;

namespace WS_Comercializadora_Dotnet_Soap_GR01.DataAccess
{
    public class ProductoDAL
    {
        public List<Producto> ObtenerTodos()
        {
            var lista = new List<Producto>();
            using (var conn = DbConnection.GetConnection())
            {
                string query = "SELECT PRO_ID, PRO_NOMBRE, PRO_DESCRIPCION, PRO_PRECIO FROM PRODUCTO";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Producto
                        {
                            ProId = Convert.ToInt32(reader["PRO_ID"]),
                            ProNombre = reader["PRO_NOMBRE"].ToString(),
                            ProDescripcion = reader["PRO_DESCRIPCION"] != DBNull.Value ? reader["PRO_DESCRIPCION"].ToString() : "",
                            ProPrecio = Convert.ToDecimal(reader["PRO_PRECIO"])
                        });
                    }
                }
            }
            return lista;
        }

        public Producto ObtenerPorId(int id)
        {
            Producto producto = null;
            using (var conn = DbConnection.GetConnection())
            {
                string query = "SELECT PRO_ID, PRO_NOMBRE, PRO_DESCRIPCION, PRO_PRECIO FROM PRODUCTO WHERE PRO_ID = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        producto = new Producto
                        {
                            ProId = Convert.ToInt32(reader["PRO_ID"]),
                            ProNombre = reader["PRO_NOMBRE"].ToString(),
                            ProDescripcion = reader["PRO_DESCRIPCION"] != DBNull.Value ? reader["PRO_DESCRIPCION"].ToString() : "",
                            ProPrecio = Convert.ToDecimal(reader["PRO_PRECIO"])
                        };
                    }
                }
            }
            return producto;
        }

        public bool Insertar(Producto p)
        {
            using (var conn = DbConnection.GetConnection())
            {
                string query = @"INSERT INTO PRODUCTO (PRO_NOMBRE, PRO_DESCRIPCION, PRO_PRECIO) 
                                VALUES (@Nombre, @Descripcion, @Precio)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", p.ProNombre);
                cmd.Parameters.AddWithValue("@Descripcion", (object)p.ProDescripcion ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Precio", p.ProPrecio);
                conn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Actualizar(Producto p)
        {
            using (var conn = DbConnection.GetConnection())
            {
                string query = @"UPDATE PRODUCTO 
                                SET PRO_NOMBRE = @Nombre, PRO_DESCRIPCION = @Descripcion, PRO_PRECIO = @Precio 
                                WHERE PRO_ID = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", p.ProId);
                cmd.Parameters.AddWithValue("@Nombre", p.ProNombre);
                cmd.Parameters.AddWithValue("@Descripcion", (object)p.ProDescripcion ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Precio", p.ProPrecio);
                conn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Eliminar(int id)
        {
            using (var conn = DbConnection.GetConnection())
            {
                string query = "DELETE FROM PRODUCTO WHERE PRO_ID = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();

                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}