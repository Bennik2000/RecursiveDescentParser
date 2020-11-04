using StateMachineCompiler.Ast;

namespace StateMachineCompiler.Visitors
{
    public abstract class Visitor
    {
        public virtual void Visit(StateTransition node)
        {
        }

        public virtual void Visit(StateTransitionList node)
        {
        }

        public virtual void Visit(State node)
        {
        }

        public virtual void Visit(EventList node)
        {
        }

        public virtual void Visit(StateMachine node)
        {
        }

        public virtual void Visit(Event node)
        {
        }

        public virtual void Visit(Action node)
        {
        }
    }
}