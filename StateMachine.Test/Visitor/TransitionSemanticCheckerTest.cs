using Microsoft.VisualStudio.TestTools.UnitTesting;
using StateMachineCompiler.Errors;
using StateMachineCompiler.Visitors;

namespace StateMachineCompiler.Test.Visitor
{
    [TestClass]
    public class TransitionSemanticCheckerTest
    {
        private void DoSemanticCheck(string input)
        {
            var parser = new Parser(input);

            var stateMachine = parser.Parse();

            stateMachine.Accept(new TransitionSemanticChecker());
        }

        [TestMethod]
        public void When_CorrectInput_Assert_NoException()
        {
            DoSemanticCheck(
                "event switch_on, switch_off, timer;                 \n" +
                "in initial state OFF:                                    \n" +
                "  on switch_on goto ON: turn_on_light, set_timer_5min;   \n" +
                "in state ON:                                             \n" +
                "  on switch_off, timer goto OFF: turn_off_light;         \n" +
                "  on switch_on goto ON: turn_off_light;                  \n");
        }

        [TestMethod]
        [ExpectedException(typeof(MultipleEventsForStateException))]
        public void When_EventNotDefined_Assert_ThrowsException()
        {
            DoSemanticCheck(
                "event switch_off, timer;                            \n" +
                "in initial state OFF:                                    \n" +
                "  on switch_on goto bar: turn_on_light;                  \n" +
                "  on switch_on goto ON: set_timer_5min;                  \n");
        }
    }
}
