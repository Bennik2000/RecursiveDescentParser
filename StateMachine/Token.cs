using System;

namespace StateMachineCompiler
{
    public class Token
    {
        public TokenType Type { get; }

        public String Value { get; }

        public Token(TokenType type, string value)
        {
            Type = type;
            Value = value;
        }
    }
}