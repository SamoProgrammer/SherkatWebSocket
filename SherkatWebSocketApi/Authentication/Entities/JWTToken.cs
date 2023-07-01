namespace SherkatWebSocketApi.Authentication.Entities;

public class JWTToken
{
    public string Token { get; set; }
    public DateTime Expires { get; set; }
}