using System.Collections.Generic;
using StateMachineCompiler.Ast;
using StateMachineCompiler.Errors;

namespace StateMachineCompiler.Visitors
{
    class SymbolTable
    {
        public Dictionary<string, Event> Events { get; set; }
        public Dictionary<string, State> States { get; set; }
        public Dictionary<string, Action> Actions { get; set; }
        
        public State InitialState { get; set; }

        public SymbolTable()
        {
            Events = new Dictionary<string, Event>();
            States = new Dictionary<string, State>();
            Actions = new Dictionary<string, Action>();
        }

        public State ResolveState(string name)
        {
            if (States.ContainsKey(name))
            {
                return States[name];
            }

            throw new StateNotFoundException();
        }

        public Event ResolveEvent(string id)
        {
            if (Events.ContainsKey(id))
            {
                return Events[id];
            }

            throw new EventNotFoundException();
        }

        public Action ResolveAction(string id)
        {
            if (Actions.ContainsKey(id))
            {
                return Actions[id];
            }

            throw new ActionNotFoundException();
        }

        public void RegisterAction(Action action)
        {
            if (!Actions.ContainsKey(action.Id))
            {
                Actions.Add(action.Id, action);
            }
        }

        public void RegisterState(State state)
        {
            var stateId = state.Id;

            if (States.ContainsKey(stateId))
            {
                throw new StateDeclaredMultipleTimesException(stateId);
            }

            States.Add(stateId, state);
        }

        public void RegisterEvent(Event ev)
        {
            if (Events.ContainsKey(ev.Id))
            {
                throw new EventDeclaredMultipleTimesException(ev.Id);
            }

            Events.Add(ev.Id, ev);
        }

        public void SetInitialState(State state)
        {
            if (InitialState != null)
            {
                throw new MultipleInitialStatesException();
            }

            InitialState = state;
        }
    }
}