using StateMachineCompiler.Ast;
using System.Linq;

namespace StateMachineCompiler.Visitors
{
    /// <summary>
    /// Resolves the events of a StateTransition
    /// </summary>
    public class EventResolver : Visitor
    {
        public SymbolTable SymbolTable { get; set; }

        public EventResolver(SymbolTable symbolTable)
        {
            SymbolTable = symbolTable;
        }

        public override void Visit(StateTransition node)
        {
            var events = node.EventIds.Select(id => SymbolTable.ResolveEvent(id));
            node.Events = events.ToList();
        }
    }
}