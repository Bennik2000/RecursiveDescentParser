using System.Linq;
using StateMachineCompiler.Ast;

namespace StateMachineCompiler.Visitors
{
    class EventResolver : Visitor
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