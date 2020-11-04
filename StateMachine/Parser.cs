using System.Collections.Generic;
using System.Linq;
using StateMachineCompiler.Ast;
using StateMachineCompiler.Errors;


namespace StateMachineCompiler
{
    public class Parser
    {
        private Scanner _scanner;
        private Token _lookahead;


        public Parser(string input)
        {
            _scanner = new Scanner(input);
        }

        public StateMachine Parse()
        {
            Consume();

            var stateMachine = StateMachine();

            Match(TokenType._EndOfInput);

            return stateMachine;
        }


        private StateMachine StateMachine()
        {
            var eventDeclarations = EventDeclaration();
            var states = States();

            return new StateMachine(
                eventDeclarations, states);
        }

        private EventList EventDeclaration()
        {
            Match(TokenType.Event);

            var idList = IdList();

            Match(TokenType.Terminator);

            var events = idList.Select(id => new Event(id)).ToList();

            return new EventList(events);
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

        private List<State> States()
        {
            var states = new List<State>();

            states.Add(State());

            if (_lookahead.Type == TokenType.In)
            {
                states.AddRange(States());
            }

            return states;
        }

        private State State()
        {
            Match(TokenType.In);
            var isInitial = OptionalInitial();
            Match(TokenType.State);
            var id = Match(TokenType.Id);
            Match(TokenType.Colon);
            var transitions = StateTransitions();

            return new State(id.Value, isInitial, transitions);
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

        private StateTransitionList StateTransitions()
        {
            var stateTransitions = new List<StateTransition>();

            stateTransitions.Add(StateTransition());

            if (_lookahead.Type == TokenType.On)
            {
                stateTransitions.AddRange(StateTransitions().Transitions);
            }

            return new StateTransitionList(stateTransitions);
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