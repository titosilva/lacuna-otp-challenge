using Lacuna.Domain.Entities;

namespace Lacuna.Domain.Services
{
    public interface IApiService
    {
        bool CreateUser(User user);
        Token Login(User user);
        string GetSecret(Token token);
    }
}