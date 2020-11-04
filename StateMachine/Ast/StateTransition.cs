using System.Collections.Generic;
using StateMachineCompiler.Visitors;

namespace StateMachineCompiler.Ast
{
    public class StateTransition : AstNode
    {
        public List<string> EventIds { get; }
        public List<string> ActionIds { get; }
        public string Target { get; }

        public State TargetState { get; set; }
        public List<Event> Events { get; set; }
        public List<Action> Actions { get; set; }

        public StateTransition(List<string> actionIds, string target, List<string> eventIds)
        {
            ActionIds = actionIds;
            Target = target;
            EventIds = eventIds;
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);
        }
    }
}