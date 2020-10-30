using System;

namespace StateMachine
{
    public enum TokenType
    {
        Event,
        Id,
        Terminator,
        Comma,
        In,
        On,
        Initial,
        State,
        Colon,
        Goto,
        Blank,
        _Invalid,
        _EndOfInput,
    }

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