using SherkatWebSocketApi.Authentication.Entities;

namespace SherkatWebSocketApi.Authentication;

public interface IJWTManagerRepository
{
    JWTToken Authenticate(User user); 
}