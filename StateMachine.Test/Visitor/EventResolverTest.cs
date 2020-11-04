using Microsoft.VisualStudio.TestTools.UnitTesting;
using StateMachineCompiler.Ast;
using StateMachineCompiler.Errors;
using StateMachineCompiler.Visitors;

namespace StateMachineCompiler.Test.Visitor
{
    [TestClass]
    public class EventResolverTest
    {
        private StateMachine buildSymbolTableAndResolveEvents(string input)
        {
            var parser = new Parser(input);

            var stateMachine = parser.Parse();

            var symbolTable = new SymbolTable();

            stateMachine.Accept(new SymbolTableBuilder(symbolTable));
            stateMachine.Accept(new EventResolver(symbolTable));

            return stateMachine;
        }

        [TestMethod]
        public void When_CorrectInput_Assert_CorrectEventsResolved()
        {
            var stateMachine = buildSymbolTableAndResolveEvents(
                "event switch_on, switch_off, timer;                 \n" +
                "in initial state OFF:                                    \n" +
                "  on switch_on goto ON: turn_on_light, set_timer_5min;   \n" +
                "in state ON:                                             \n" +
                "  on switch_off, timer goto OFF: turn_off_light;         \n" +
                "  on switch_on goto ON: turn_off_light;                  \n");


            Assert.AreSame(
                stateMachine.EventList.Events[0],
                stateMachine.States[0].TransitionList.Transitions[0].Events[0]
            );

            Assert.AreSame(
                stateMachine.EventList.Events[1],
                stateMachine.States[1].TransitionList.Transitions[0].Events[0]
            );

            Assert.AreSame(
                stateMachine.EventList.Events[2],
                stateMachine.States[1].TransitionList.Transitions[0].Events[1]
            );

            Assert.AreSame(
                stateMachine.EventList.Events[0],
                stateMachine.States[1].TransitionList.Transitions[1].Events[0]
            );
        }

        [TestMethod]
        [ExpectedException(typeof(EventNotFoundException))]
        public void When_EventNotDefined_Assert_ThrowsException()
        {
            buildSymbolTableAndResolveEvents(
                "event switch_off, timer;                            \n" +
                "in initial state OFF:                                    \n" +
                "  on switch_on goto bar: turn_on_light, set_timer_5min;  \n");
        }
    }
}
