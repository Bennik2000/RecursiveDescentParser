using System.Collections.Generic;


namespace StateMachine
{
    public class Parser
    {
        private Scanner _scanner;
        private Token _lookahead;


        public Parser(string input)
        {
            _scanner = new Scanner(input);
        }

        public StateMachineModel Parse()
        {
            Consume();

            var stateMachine = StateMachine();

            Match(TokenType._EndOfInput);

            return stateMachine;
        }


        private StateMachineModel StateMachine()
        {
            var eventDeclarations = EventDeclaration();
            var stateDeclarations = StateDeclarations();

            return new StateMachineModel(
                eventDeclarations, stateDeclarations);
        }

        private EventDeclaration EventDeclaration()
        {
            Match(TokenType.Event);

            var idList = IdList();

            Match(TokenType.Terminator);

            return new EventDeclaration(idList);
        }

        private List<string> IdList()
        {
            var ids = new List<string>();

            ids.Add(Id());

            if (_lookahead.Type == TokenType.Comma)
            {
                Consume();
                ids.AddRange(IdList());
            }

            return ids;
        }

        private string Id()
        {
            var token = Match(TokenType.Id);
            return token.Value;
        }

        private List<StateDeclaration> StateDeclarations()
        {
            var stateDeclarations = new List<StateDeclaration>();

            stateDeclarations.Add(StateDeclaration());

            if (_lookahead.Type == TokenType.In)
            {
                stateDeclarations.AddRange(StateDeclarations());
            }

            return stateDeclarations;
        }

        private StateDeclaration StateDeclaration()
        {
            Match(TokenType.In);
            var isInitial = OptionalInitial();
            Match(TokenType.State);
            var id = Match(TokenType.Id);
            Match(TokenType.Colon);
            var transitions = StateTransition();

            return new StateDeclaration(id.Value, isInitial, transitions);
        }

        private bool OptionalInitial()
        {
            if (_lookahead.Type == TokenType.Initial)
            {
                Consume();

                return true;
            }

            return false;
        }

        private StateTransition StateTransition()
        {
            Match(TokenType.On);
            var triggers = IdList();
            Match(TokenType.Goto);
            var target = Id();
            Match(TokenType.Colon);
            var events = IdList();
            Match(TokenType.Terminator);

            return new StateTransition(events, target, triggers);
        }


        private Token Match(TokenType token)
        {
            var currentToken = _lookahead;

            if (_lookahead.Type == token)
            {
                Consume();
            }
            else
            {
                ThrowSyntaxError();
            }

            return currentToken;
        }

        private void Consume()
        {
            _lookahead = _scanner.GetNextToken();
        }

        private void ThrowSyntaxError()
        {
            throw new SyntaxErrorException();
        }
    }
}