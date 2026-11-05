using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace ec.edu.monster.Seguridad
{
    public class TokenHeaderEndpointBehavior : IEndpointBehavior
    {
        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new TokenHeaderDispatchInspector());
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }
}

