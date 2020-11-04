using System.Collections.Generic;
using StateMachineCompiler.Visitors;

namespace StateMachineCompiler.Ast
{
    public class StateTransitionList : AstNode
    { 
        public List<StateTransition> Transitions { get; }

        public StateTransitionList(List<StateTransition> transitions)
        {
            Transitions = transitions;
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);

            foreach (var transition in Transitions)
            {
                transition.Accept(visitor);
            }
        }
    }
}