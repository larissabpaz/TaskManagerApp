public record class JwtOptions
{
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string SigningKey { get; set; } = string.Empty;
    public int ExpirationSeconds { get; set; }

    public JwtOptions() { } 

    public JwtOptions(string issuer, string audience, string signingKey, int expirationSeconds)
    {
        Issuer = issuer;
        Audience = audience;
        SigningKey = signingKey;
        ExpirationSeconds = expirationSeconds;
    }
}
