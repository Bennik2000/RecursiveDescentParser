using StateMachineCompiler.Ast;
using Action = StateMachineCompiler.Ast.Action;

namespace StateMachineCompiler.Visitors
{
    class SymbolTableBuilder : Visitor
    {
        public SymbolTable SymbolTable { get; }

        public SymbolTableBuilder(SymbolTable symbolTable)
        {
            SymbolTable = symbolTable;
        }
        
        public override void Visit(State node)
        {
            SymbolTable.RegisterState(node);

            if (!node.IsInitial) return;
            SymbolTable.SetInitialState(node);
        }

        public override void Visit(Event node)
        {
            SymbolTable.RegisterEvent(node);
        }

        public override void Visit(StateTransition node)
        {
            foreach (var id in node.ActionIds)
            {
                SymbolTable.RegisterAction(new Action(id));
            }
        }
    }
}