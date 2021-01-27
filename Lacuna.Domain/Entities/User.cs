using System;
using Lacuna.Shared.Entities;

namespace Lacuna.Domain.Entities
{
    public class User : Validatable
    {
        public User(string name, string password)
        {
            Name = name;
            Password = password;
        }

        public string Name { get; private set; }
        public string Password { get; private set; }

        public static string GenerateRandomName(int size=32)
        {
            // Using Guid generator to build a random name
            return Guid.NewGuid().ToString().Substring(0, size).Replace("-", "x");
        }

        public override void Validate()
        {
            if(Name.Length<8 || Name.Length>32)
            {
                isInvalid = true;
            }
        }
    }
}