using StateMachineCompiler.Ast;
using StateMachineCompiler.Errors;
using System.Collections.Generic;

namespace StateMachineCompiler.Visitors
{
    /// <summary>
    /// Does a semantic check for the state transitions
    ///
    /// An invalid transition would be if a state already has a transition defined for a event:
    ///
    /// in initial state OFF:                 
    ///   on switch_on goto bar: turn_on_light
    ///   on switch_on goto ON: set_timer_5min
    /// 
    /// </summary>
    public class TransitionSemanticChecker : Visitor
    {
        private readonly List<string> _eventIds = new List<string>(); 

        public override void Visit(State node)
        {
            _eventIds.Clear();
        }

        public override void Visit(StateTransition node)
        {
            foreach (var eventId in node.EventIds)
            {
                if (_eventIds.Contains(eventId))
                {
                    throw new MultipleEventsForStateException();
                }

                _eventIds.Add(eventId);
            }
        }
    }
}