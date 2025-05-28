using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace APIWorkshop.Authentication;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private const string STAFF_USERNAME = "staff";
    private const string STAFF_PASSWORD = "BCLyon2024";

    public BasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
        {
            return AuthenticateResult.Fail("Authorization header not found.");
        }

        try
        {
            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            if (authHeader.Scheme != "Basic")
            {
                return AuthenticateResult.Fail("Authentication scheme must be Basic.");
            }

            var credentialBytes = Convert.FromBase64String(authHeader.Parameter ?? string.Empty);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);
            var username = credentials[0];
            var password = credentials[1];

            if (username != STAFF_USERNAME || password != STAFF_PASSWORD)
            {
                return AuthenticateResult.Fail("Invalid username or password.");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "Staff")
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
        catch
        {
            return AuthenticateResult.Fail("Invalid authorization header.");
        }
    }
}