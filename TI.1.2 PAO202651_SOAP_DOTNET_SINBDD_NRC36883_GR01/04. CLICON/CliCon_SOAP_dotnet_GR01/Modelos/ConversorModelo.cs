using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using WSMonsterReference;

namespace ClICON.Modelos
{
    public class ConversorModelo
    {
        private readonly WSConversorUnidadesClient _clienteSoap;
        public string Token { get; private set; }

        public ConversorModelo()
        {
            var textEncoding = new TextMessageEncodingBindingElement(MessageVersion.Soap11, System.Text.Encoding.UTF8);
            var httpTransport = new HttpTransportBindingElement();

            // 2. EL FIX: Pasamos los elementos directamente en el constructor del CustomBinding 
            // en lugar de usar .Elements.Add(). Esto evita el ArrayTypeMismatchException en .NET moderno.
            var customBinding = new CustomBinding(textEncoding, httpTransport);

            var endpoint = new EndpointAddress("http://localhost:9099/WSConversorUnidades.svc");

            _clienteSoap = new WSConversorUnidadesClient(customBinding, endpoint);
        }

        public async Task<bool> IniciarSesionAsync(string usuario, string contrasenia)
        {
            try
            {
                // El login no requiere cabecera según el TokenHeaderDispatchInspector
                Token = await _clienteSoap.loginAsync(usuario, contrasenia);
                return !string.IsNullOrWhiteSpace(Token);
            }
            catch (FaultException ex)
            {
                // Atrapa el throw new FaultException("Credenciales incorrectas") del backend
                Console.WriteLine($"[Servidor rechazó la petición]: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> CambiarContraseniaAsync(string contraseniaActual, string nuevaContrasenia)
        {
            // Nota: Revisa si tu referencia de Visual Studio generó el método con mayúscula o minúscula.
            // Usualmente es CambiarContraseniaAsync o cambiarContraseniaAsync.
            return await EjecutarConTokenAsync(() => _clienteSoap.cambiarContraseniaAsync(contraseniaActual, nuevaContrasenia));
        }

        public void CerrarSesion()
        {
            Token = null;
        }

        public async Task<double> ConvertirLongitudAsync(double valor, string origen, string destino)
        {
            return await EjecutarConTokenAsync(() => _clienteSoap.convertirLongitudAsync(valor, origen, destino));
        }

        public async Task<double> ConvertirMasaAsync(double valor, string origen, string destino)
        {
            return await EjecutarConTokenAsync(() => _clienteSoap.convertirMasaAsync(valor, origen, destino));
        }

        public async Task<double> ConvertirTemperaturaAsync(double valor, string origen, string destino)
        {
            return await EjecutarConTokenAsync(() => _clienteSoap.convertirTemperaturaAsync(valor, origen, destino));
        }

        // Método auxiliar para inyectar el Token en la cabecera SOAP
        private async Task<T> EjecutarConTokenAsync<T>(Func<Task<T>> operacion)
        {
            if (string.IsNullOrEmpty(Token))
                throw new InvalidOperationException("Debes iniciar sesión primero.");

            // Abre un ámbito para modificar el contexto de la petición actual
            using (new OperationContextScope(_clienteSoap.InnerChannel))
            {
                // Crea la cabecera con el nombre "token" que busca tu TryReadToken en el backend
                MessageHeader header = MessageHeader.CreateHeader("token", "", Token);
                OperationContext.Current.OutgoingMessageHeaders.Add(header);

                // Ejecuta la petición
                return await operacion();
            }
        }
    }
}
