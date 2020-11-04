using StateMachineCompiler.Ast;

namespace StateMachineCompiler.Visitors
{
    class StateResolver : Visitor
    {
        public SymbolTable SymbolTable { get; set; }

        public StateResolver(SymbolTable symbolTable)
        {
            SymbolTable = symbolTable;
        }

        public override void Visit(StateTransition node)
        {
            var targetState = SymbolTable.ResolveState(node.Target);
            node.TargetState = targetState;
        }
    }
}
