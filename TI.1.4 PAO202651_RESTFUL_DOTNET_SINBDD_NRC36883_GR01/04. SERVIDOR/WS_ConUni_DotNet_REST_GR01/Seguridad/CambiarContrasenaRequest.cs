namespace WS_ConUni_DotNet_Rest_GR5.Seguridad;

public class CambiarContrasenaRequest
{
    public string ContrasenaActual { get; set; } = string.Empty;
    public string NuevaContrasena { get; set; } = string.Empty;
}
