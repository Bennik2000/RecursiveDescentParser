using StateMachineCompiler.Visitors;

namespace StateMachineCompiler.Ast
{
    public class Action : Identifiable
    {
        public Action(string id) : base(id)
        {

        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
