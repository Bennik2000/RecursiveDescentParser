using StateMachineCompiler.Visitors;

namespace StateMachineCompiler.Ast
{
    public abstract class Identifiable : AstNode
    {
        public string Id { get; }

        protected Identifiable(string id)
        {
            Id = id;
        }

        public abstract override void Accept(Visitor visitor);
    }
}
