using StateMachineCompiler.Ast;
using StateMachineCompiler.Visitors;
using System;
using System.Linq;
using System.Text;

namespace StateMachineCompiler.CodeGeneration
{
    public class CCodeGenerator : CodeGenerator
    {
        public CCodeGenerator(StateMachine stateMachine, SymbolTable symbolTable) : base(stateMachine, symbolTable)
        {
        }

        protected override void Generate(StringBuilder stringBuilder)
        {
            GenerateStateEnum(stringBuilder);
            GenerateEventEnum(stringBuilder);

            stringBuilder.AppendLine();

            GenerateHandleEventMethod(stringBuilder);
            stringBuilder.AppendLine();
        }

        private void GenerateStateEnum(StringBuilder stringBuilder)
        {
            stringBuilder.Append("enum STATE { ");

            var states = SymbolTable.States.Values.ToList();

            for (int i = 0; i < states.Count; i++)
            {
                stringBuilder.Append(states[i].Id);

                if (i < states.Count - 1)
                {
                    stringBuilder.Append(", ");
                }
            }

            stringBuilder.Append(" } the_state = ");
            stringBuilder.Append(SymbolTable.InitialState.Id);
            stringBuilder.Append(";");

            stringBuilder.AppendLine();
        }

        private void GenerateEventEnum(StringBuilder stringBuilder)
        {
            stringBuilder.Append("enum EVENT { ");

            var events = SymbolTable.Events.Values.ToList();

            for (int i = 0; i < events.Count; i++)
            {
                stringBuilder.Append(events[i].Id);

                if (i < events.Count - 1)
                {
                    stringBuilder.Append(", ");
                }
            }

            stringBuilder.Append(" }");
            stringBuilder.AppendLine();
        }

        private void GenerateHandleEventMethod(StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine("void handleEvent(enum EVENT ev) {");
            stringBuilder.AppendLine("  switch (the_state) {");

            foreach (var state in SymbolTable.States.Values)
            {
                GenerateStateCase(stringBuilder, state);
            }

            stringBuilder.AppendLine("  }");
            stringBuilder.AppendLine("}");
        }

        private void GenerateStateCase(StringBuilder stringBuilder, State state)
        {
            stringBuilder.Append("  case ");
            stringBuilder.Append(state.Id);
            stringBuilder.Append(":");
            stringBuilder.AppendLine();

            foreach (var transition in state.TransitionList.Transitions)
            {
                GenerateTransitionSwitch(stringBuilder, transition);
            }
            
            stringBuilder.Append("    break;");
            stringBuilder.AppendLine();
        }

        private void GenerateTransitionSwitch(StringBuilder stringBuilder, StateTransition transition)
        {
            stringBuilder.Append("    switch (ev) {");
            stringBuilder.AppendLine();

            foreach (var ev in transition.Events)
            {
                stringBuilder.Append("    case ");
                stringBuilder.Append(ev.Id);
                stringBuilder.Append(":");
                stringBuilder.AppendLine();
            }
            stringBuilder.Append("      the_state = ");
            stringBuilder.Append(transition.TargetState.Id);
            stringBuilder.Append(";");
            stringBuilder.AppendLine();

            foreach (var action in transition.Actions)
            {
                stringBuilder.Append("      ");
                stringBuilder.Append(action.Id);
                stringBuilder.Append("();");
                stringBuilder.AppendLine();
            }

            stringBuilder.Append("      break;");
            stringBuilder.AppendLine();

            stringBuilder.AppendLine("    default:");
            stringBuilder.AppendLine("      error(the_state, ev);");
            stringBuilder.AppendLine("    }");
        }
    }
}
