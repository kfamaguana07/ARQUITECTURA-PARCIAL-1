using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace WS_ConUni_DotNet_Rest_GR5.Seguridad;

public class BearerTokenAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly AuthService _authService;

    public BearerTokenAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        AuthService authService)
        : base(options, logger, encoder, clock)
    {
        _authService = authService;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("Authorization", out var authHeader))
        {
            return Task.FromResult(AuthenticateResult.Fail("No se encontró el encabezado Authorization"));
        }

        var valorAuthorization = authHeader.ToString();
        const string prefijoBearer = "Bearer ";
        if (!valorAuthorization.StartsWith(prefijoBearer, StringComparison.OrdinalIgnoreCase))
        {
            return Task.FromResult(AuthenticateResult.Fail("Tipo de autorización inválido"));
        }

        var token = valorAuthorization[prefijoBearer.Length..].Trim();
        if (string.IsNullOrWhiteSpace(token) || !_authService.EsTokenValido(token))
        {
            return Task.FromResult(AuthenticateResult.Fail("Token inválido o expirado"));
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, _authService.UsuarioAutorizado)
        };

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
