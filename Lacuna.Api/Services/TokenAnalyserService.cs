using System;
using Lacuna.Domain.Entities;
using Lacuna.Domain.Services;
using Lacuna.Shared.Entities;

namespace Lacuna.Api.Services
{
    public class TokenAnalyser : ITokenAnalyser
    {
        private IApiService _apiService;
        private readonly int _usernameMaxLength = 32;
        private readonly int _usernameMinLength = 8;
        public TokenAnalyser(IApiService apiService)
        {
            _apiService = apiService;
        }

        public Binary GenerateUsernameMask(string username, int usernamePosition, int tokenSize, char padding = '\0')
        {
            var result = new Binary();
            // Zeroes before the username
            for (int i = 0; i < usernamePosition; i++)
                result.Insert(0, 0);

            if(username.Length<=_usernameMaxLength)
                result.Concatenate(Binary.FromString(username));
            else
                result.Concatenate(Binary.FromString(username.Substring(0, _usernameMaxLength)));

            var paddingConverted = Convert.ToByte(padding);
            for (int i = username.Length; i < _usernameMaxLength; i++)
                result.Add(paddingConverted);

            while (result.Size() < tokenSize)
                result.Add(0);

            return result;
        }

        public int DiscoverUsernamePosition()
        {
            // Generate random user names
            var username1 = User.GenerateRandomName(_usernameMaxLength);
            var username2 = User.GenerateRandomName(_usernameMaxLength);

            // Create the users
            var user1 = new User(username1, username1);
            var user2 = new User(username2, username2);

            if (!_apiService.CreateUser(user1) || !_apiService.CreateUser(user2))
                throw new Exception("Could not create users");

            // Login
            var token1 = _apiService.Login(user1);
            var token2 = _apiService.Login(user2);

            if (token1 == null || token2 == null)
                throw new Exception("Could not login");

            // XOR Tokens (defined by the Binary class)
            var tokenxor = token1 ^ token2;

            // XOR Usernames
            var usernamexor = Binary.FromString(user1.Name) ^ Binary.FromString(user2.Name);

            // Match XORs
            var match = tokenxor.Match(usernamexor);

            return match;
        }

        public Binary DiscoverTokenMask(int usernamePosition)
        {
            var name = User.GenerateRandomName(_usernameMaxLength);
            var user = new User(name, name);

            if(!_apiService.CreateUser(user))
                throw new Exception("Could not create user");

            var token = _apiService.Login(user);

            if(token==null)
                throw new Exception("Could not login");

            var usernameMask = GenerateUsernameMask(user.Name, usernamePosition, token.Size());

            return usernameMask ^ token;
        }

        public char DiscoverPaddingChar(int usernamePosition, Binary tokenMask)
        {
            var name = User.GenerateRandomName(_usernameMinLength);
            var user = new User(name, name);

            if(!_apiService.CreateUser(user))
                throw new Exception("Could not create user");

            var token = _apiService.Login(user);

            if(token==null)
                throw new Exception("Could not login");

            var partial = tokenMask ^ token;

            var padding = Convert.ToChar(partial.AtIndex(usernamePosition+name.Length+1));

            return padding;
        }

        public string ForgeToken(string username, int usernamePosition, Binary tokenMask, char padding)
        {
            var usernameMask = GenerateUsernameMask(username, usernamePosition, tokenMask.Size(), padding);

            return (tokenMask ^ usernameMask).ToString();
        }
    }
}
