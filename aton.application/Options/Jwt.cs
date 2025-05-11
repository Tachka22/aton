namespace aton.application.Options;

public class Jwt
{
    public string Key { get; set; } 
    public string Issuer { get; set; } 
    public string Audience { get; set; } 
    public int TokenValidityInDay { get; set; }
    public int RefreshTokenValidityInDay { get; set; }
}
