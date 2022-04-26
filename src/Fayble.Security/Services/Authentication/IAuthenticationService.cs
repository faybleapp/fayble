using Fayble.Security.Models;

namespace Fayble.Security.Services.Authentication;

public interface IAuthenticationService
{
    Task<User> GetUser(Guid id);
    Task<AuthenticationResult> Login(LoginCredentials loginCredentials);
    Task<User> CreateUser(NewUser newUser);
    Task<AuthenticationResult> RefreshToken(string refreshToken);
    Task<User> GetCurrentUser();
}