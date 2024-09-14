namespace AlunosApi.Interfaces;

public interface IAuthenticationService
{
    Task<bool> Authenticate(string email, string password);
    Task<bool> Register(string email, string password);
    Task Logout();
}
