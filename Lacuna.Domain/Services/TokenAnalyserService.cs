using Lacuna.Domain.Entities;
using Lacuna.Shared.Entities;

namespace Lacuna.Domain.Services
{
    public interface ITokenAnalyser
    {
        int DiscoverUsernamePosition();
        Binary DiscoverTokenMask(int usernamePosition);
        char DiscoverPaddingChar(int usernamePosition, Binary tokenMask);
        string ForgeToken(string username, int usernamePosition, Binary tokenMask, char padding);
        string GetSecret(Token token);
        Binary GenerateUsernameMask(string username, int usernamePosition, int tokenSize, char padding = '\0');
    }
}