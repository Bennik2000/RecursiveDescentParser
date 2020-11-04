using Microsoft.VisualStudio.TestTools.UnitTesting;
using StateMachineCompiler.Ast;
using StateMachineCompiler.Errors;
using StateMachineCompiler.Visitors;

namespace StateMachineCompiler.Test.Visitor
{
    [TestClass]
    public class StateResolverTest
    {
        private StateMachine buildSymbolTableAndResolveNames(string input)
        {
            var parser = new Parser(input);

            var stateMachine = parser.Parse();

            var symbolTable = new SymbolTable();

            stateMachine.Accept(new SymbolTableBuilder(symbolTable));
            stateMachine.Accept(new StateResolver(symbolTable));

            return stateMachine;
        }

        [TestMethod]
        public void When_CorrectInput_Assert_CorrectStateResolved()
        {
            var stateMachine = buildSymbolTableAndResolveNames(
                "event switch_on, switch_off, timer;                 \n" +
                "in initial state OFF:                                    \n" +
                "  on switch_on goto ON: turn_on_light, set_timer_5min;   \n" +
                "in state ON:                                             \n" +
                "  on switch_off, timer goto OFF: turn_off_light;         \n" +
                "  on switch_on goto ON: turn_off_light;                  \n");


            Assert.AreSame(
                stateMachine.States[0],
                stateMachine.States[1].TransitionList.Transitions[0].TargetState
            );

            Assert.AreSame(
                stateMachine.States[1],
                stateMachine.States[0].TransitionList.Transitions[0].TargetState
            );

            Assert.AreSame(
                stateMachine.States[1],
                stateMachine.States[1].TransitionList.Transitions[1].TargetState
            );
        }

        [TestMethod]
        [ExpectedException(typeof(StateNotFoundException))]
        public void When_StateNotDefined_Assert_ThrowsException()
        {
            buildSymbolTableAndResolveNames(
                "event switch_on, switch_off, timer;                 \n" +
                "in initial state OFF:                                    \n" +
                "  on switch_on goto bar: turn_on_light, set_timer_5min;  \n");
        }
    }
}