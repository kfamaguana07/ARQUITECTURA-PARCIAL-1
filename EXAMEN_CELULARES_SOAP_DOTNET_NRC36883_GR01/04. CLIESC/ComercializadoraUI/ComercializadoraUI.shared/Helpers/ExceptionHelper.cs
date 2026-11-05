using System;
using System.ServiceModel;

namespace ComercializadoraUI.shared.Helpers
{
    public static class ExceptionHelper
    {
        public static string ObtenerMensajeAmigable(Exception ex)
        {
            if (ex == null) return "Ocurrió un error inesperado.";

            string mensajeOriginal = ex.Message;

            // 1. Violación de Clave Foránea (Intentar borrar un registro con ventas asociadas)
            if (mensajeOriginal.Contains("FK_POSEE_PRODUCTO") ||
                mensajeOriginal.Contains("REFERENCE constraint") ||
                mensajeOriginal.Contains("DELETE statement conflicted"))
            {
                return "No se puede eliminar este producto porque ya tiene ventas o facturas asociadas en el historial.";
            }

            // 2. Error de Conexión WCF / Red
            if (ex is EndpointNotFoundException || mensajeOriginal.Contains("Could not connect") || mensajeOriginal.Contains("there was no endpoint listening"))
            {
                return "No se pudo establecer conexión con el servicio WCF. Verifica que BanQuito Core / Comercializadora esté en ejecución.";
            }

            // 3. Timeouts WCF
            if (ex is TimeoutException || mensajeOriginal.Contains("timed out"))
            {
                return "La respuesta del servidor WCF tardó demasiado tiempo. Inténtalo de nuevo.";
            }

            // 4. Excepciones con mensaje personalizado explícito
            if (!string.IsNullOrWhiteSpace(ex.Message) && !ex.Message.Contains("System.Data") && !ex.Message.Contains("dbo."))
            {
                return ex.Message;
            }

            return "No se pudo completar la operación debido a una restricción en el sistema.";
        }
    }
}