using StateMachineCompiler.Ast;
using System.Linq;

namespace StateMachineCompiler.Visitors
{
    /// <summary>
    /// Resolves the actions of a StateTransition
    /// </summary>
    public class ActionResolver : Visitor
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
