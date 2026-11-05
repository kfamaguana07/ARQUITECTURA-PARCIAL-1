using System.ServiceModel;

namespace WS_Eurekabank_Dotnet_Soap_GR01.Interfaces
{
    [ServiceContract]
    public interface ILoginService
    {
        [OperationContract]
        bool Login(string usuario, string clave);
    }
}
