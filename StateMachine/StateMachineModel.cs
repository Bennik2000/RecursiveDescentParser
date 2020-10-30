using System.Collections.Generic;

namespace StateMachine
{
    public class StateMachineModel
    {
        public EventDeclaration EventDeclaration { get; }
        public List<StateDeclaration> StateDeclarations { get; }

        public StateMachineModel(EventDeclaration eventDeclaration, List<StateDeclaration> stateDeclarations)
        {
            EventDeclaration = eventDeclaration;
            StateDeclarations = stateDeclarations;
        }
    }

    public class EventDeclaration
    {
        public List<string> EventIds { get; }

        public EventDeclaration(List<string> eventIds)
        {
            EventIds = eventIds;
        }
    }

    public class StateDeclaration
    {
        public StateDeclaration(string id, bool isInitial, StateTransition transition)
        {
            Id = id;
            IsInitial = isInitial;
            Transition = transition;
        }

        public bool IsInitial { get; }
        public string Id { get; }
        public StateTransition Transition { get; }
    }

    public class StateTransition
    {
        public List<string> Triggers { get; }
        public string Target { get; }
        public List<string> Events { get; }

        public StateTransition(List<string> events, string target, List<string> triggers)
        {
            Events = events;
            Target = target;
            Triggers = triggers;
        }
    }
}