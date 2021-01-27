using System;
using Lacuna.Shared.Entities;

namespace Lacuna.Api.Commands
{
    public class CreateUserCommand : Validatable
    {
        public CreateUserCommand() {}
        public CreateUserCommand(string name, string password)
        {
            Name = name;
            Password = password;
        }

        public string Name { get;  set; }
        public string Password { get;  set; }

        public override void Validate()
        {
            if(Name.Length<8 || Name.Length>32)
            {
                isInvalid = true;
            }
        }
    }
}