using System.Collections.Generic;
using StateMachineCompiler.Visitors;

namespace StateMachineCompiler.Ast
{
    public class EventList : AstNode
    {
        public List<Event> Events { get; }

        public EventList(List<Event> events)
        {
            Events = events;
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);

            foreach (var ev in Events)
            {
                ev.Accept(visitor);
            }
        }
    }
}