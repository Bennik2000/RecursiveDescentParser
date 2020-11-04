using System.Collections.Generic;
using StateMachineCompiler.Visitors;

namespace StateMachineCompiler.Ast
{
    public class StateMachine : AstNode
    {
        public EventList EventList { get; }
        public List<State> States { get; }

        public StateMachine(EventList eventList, List<State> states)
        {
            EventList = eventList;
            States = states;
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);

            EventList.Accept(visitor);

            foreach(var state in States)
            {
                state.Accept(visitor);
            }
        }
    }
}