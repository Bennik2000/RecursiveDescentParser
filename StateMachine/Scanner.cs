using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace StateMachine
{
    public class Scanner
    {
        private Dictionary<TokenType, string> _terminals = new Dictionary<TokenType, string>
        {
            {TokenType.Event, "event"},
            {TokenType.Terminator, ";"},
            {TokenType.Comma, ","},
            {TokenType.Initial, "initial"},
            {TokenType.In, "in"},
            {TokenType.On, "on"},
            {TokenType.State, "state"},
            {TokenType.Colon, ":"},
            {TokenType.Goto, "goto"},
            {TokenType.Id, "[a-zA-Z_]+[0-9a-zA-Z_]*"},
            {TokenType.Blank, "[ \n]+"},
        };

        private Dictionary<TokenType, Regex> _expressions = new Dictionary<TokenType, Regex>();

        private string _input;

        private int _position;

        public Scanner(string input)
        {
            _input = input;

            _initializeRegularExpressions();
        }

        private void _initializeRegularExpressions()
        {
            foreach (var terminal in _terminals)
            {
                _expressions.Add(terminal.Key, new Regex(terminal.Value));
            }
        }

        public Token GetNextToken()
        {
            // Test all defined regular expressions
            foreach (var regex in _expressions)
            {
                var match = regex.Value.Match(_input, _position);

                if (!match.Success || match.Index != _position) continue;
                
                _position += match.Length;

                if (regex.Key == TokenType.Blank)
                {
                    return GetNextToken();
                }

                return new Token(regex.Key, match.Value);
            }

            if (_position == _input.Length)
            {
                return new Token(TokenType._EndOfInput, "");
            }


            // No token found -> no valid word
            
            return new Token(TokenType._Invalid, "");

        }
    }
}
