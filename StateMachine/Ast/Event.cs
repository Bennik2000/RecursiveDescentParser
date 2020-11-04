using StateMachineCompiler.Visitors;

namespace StateMachineCompiler.Ast
{
    public class Event : Identifiable
    {
        public Event(string id) : base(id)
        {

        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);
        }
    }
}