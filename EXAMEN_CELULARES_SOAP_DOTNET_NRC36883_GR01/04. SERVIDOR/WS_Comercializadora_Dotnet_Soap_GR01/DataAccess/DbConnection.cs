using System.Configuration;
using System.Data.SqlClient;

namespace WS_Comercializadora_Dotnet_Soap_GR01.DataAccess
{
    public static class DbConnection
    {
        public static string ConnectionString =>
            ConfigurationManager.ConnectionStrings["ComercializadoraConnStr"].ConnectionString;

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}