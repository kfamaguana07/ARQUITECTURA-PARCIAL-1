using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace ec.edu.monster.Seguridad
{
    public class TokenHeaderDispatchInspector : IDispatchMessageInspector
    {
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            var action = OperationContext.Current != null
                ? OperationContext.Current.IncomingMessageHeaders.Action
                : string.Empty;

            if (!string.IsNullOrWhiteSpace(action) && action.EndsWith("/login", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            string token;
            if (!TryReadToken(request.Headers, out token))
            {
                throw new FaultException("Acceso denegado: No se encontro el token. Inicie sesion primero.");
            }

            if (!AdministradorTokens.ValidarToken(token))
            {
                throw new FaultException("Acceso denegado: Token invalido o expirado.");
            }

            return null;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
        }

        private static bool TryReadToken(MessageHeaders headers, out string token)
        {
            for (var i = 0; i < headers.Count; i++)
            {
                if (string.Equals(headers[i].Name, "token", StringComparison.OrdinalIgnoreCase))
                {
                    using (var reader = headers.GetReaderAtHeader(i))
                    {
                        token = reader.ReadElementContentAsString()?.Trim();
                    }
                    return true;
                }
            }

            token = null;
            return false;
        }
    }
}

