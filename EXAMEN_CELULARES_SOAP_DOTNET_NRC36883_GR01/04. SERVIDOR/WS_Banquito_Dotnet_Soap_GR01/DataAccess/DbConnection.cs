using System.Configuration;
using System.Data.SqlClient;

namespace WS_Banquito_Dotnet_Soap_GR01.DataAccess
{
    public static class DbConnection
    {
        // Lee la cadena de conexión definida en el Web.config bajo la llave "BanquitoConnStr"
        public static string ConnectionString =>
            ConfigurationManager.ConnectionStrings["BanquitoConnStr"].ConnectionString;

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}