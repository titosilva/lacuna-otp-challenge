using System;
using Lacuna.Shared.Entities;

namespace Lacuna.Api.Commands
{
    public class LoginCommand : Validatable
    {
        public LoginCommand() {}
        public LoginCommand(string name, string password)
        {
            Name = name;
            Password = password;
        }

        public string Name { get;  set; }
        public string Password { get;  set; }

        public override void Validate()
        {
            isInvalid = false;
        }
    }
}