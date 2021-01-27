using System;
using System.Collections.Generic;
using Lacuna.Shared.Entities;

namespace Lacuna.Domain.Entities
{
    public class Token : Binary
    {
        public Token(string hexString) : base(hexString)
        { }
    }
}