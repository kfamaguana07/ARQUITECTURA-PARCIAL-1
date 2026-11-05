using System;
using System.Web;

namespace WS_Eurekabank_Dotnet_Soap_GR01
{
    public class Global : HttpApplication
    {
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            // Si es una petición OPTIONS (Preflight de CORS), la respondemos y terminamos
            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept, SOAPAction");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "POST, GET, OPTIONS");
                HttpContext.Current.Response.StatusCode = 200;
                HttpContext.Current.Response.End();
            }
        }
    }
}