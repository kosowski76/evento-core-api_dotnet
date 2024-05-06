namespace Evento.Infrastructure.Settings;

public class JwtSettings
{
    public const string Jwt = "Jwt";

    public string? Key { get; set; } = String.Empty;
    public string? Issuer { get; set; } = String.Empty;
    public int ExpiryMinutes { get; set; }
}
