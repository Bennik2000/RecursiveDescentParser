using System.Collections.Generic;
using StateMachineCompiler.Ast;

namespace StateMachineCompiler.Visitors
{
    public class TransitionSemanticChecker : Visitor
    {
        private List<string> _eventIds; 

        public override void Visit(State node)
        {
            _eventIds = new List<string>();
        }

        public override void Visit(StateTransition node)
        {
            foreach (var eventId in node.EventIds)
            {
                if (_eventIds.Contains(eventId))
                {
                    throw new MultipleEventsForState();
                }

                _eventIds.Add(eventId);
            }
        }
    }
}