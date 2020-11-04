using System.Linq;
using StateMachineCompiler.Ast;

namespace StateMachineCompiler.Visitors
{
    class ActionResolver : Visitor
    {
        private SymbolTable SymbolTable { get; }

        public ActionResolver(SymbolTable symbolTable)
        {
            SymbolTable = symbolTable;
        }

        public override void Visit(StateTransition node)
        {
            var actions = node.ActionIds.Select(id => SymbolTable.ResolveAction(id));
            node.Actions = actions.ToList();
        }
    }
}
