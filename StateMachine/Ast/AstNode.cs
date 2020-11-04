using StateMachineCompiler.Visitors;

namespace StateMachineCompiler.Ast
{
    public abstract class AstNode
    {
        public abstract void Accept(Visitor visitor);
    }
}