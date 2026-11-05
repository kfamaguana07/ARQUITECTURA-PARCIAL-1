namespace WS_ConUni_DotNet_Rest_GR5.Seguridad;

public static class CorsExtensions
{
    public const string PoliticaCORS = "PoliticaCorsGeneral";

    public static IServiceCollection AddCorsConfigurado(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(PoliticaCORS, policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        return services;
    }
}
