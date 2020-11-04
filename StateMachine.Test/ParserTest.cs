using Microsoft.VisualStudio.TestTools.UnitTesting;
using StateMachineCompiler.Errors;

namespace StateMachineCompiler.Test
{
    [TestClass]
    public class ParserTest
    {
        [TestMethod]
        public void When_ValidInput_Assert_Correctly_Parsed()
        {
            var validInput = "event switch_on, switch_off, timer;\n" +
                             "in initial state OFF:                           \n" +
                             "    on switch_on goto ON: turn_on_light, set_timer_5min;\n" +
                             "in state ON:                           \n" +
                             "    on switch_off, timer goto OFF: turn_off_light;\n";


            var parser = new Parser(validInput);

            var result = parser.Parse();

            Assert.AreEqual(3, result.EventList.Events.Count);
            Assert.AreEqual("switch_on", result.EventList.Events[0].Id);
            Assert.AreEqual("switch_off", result.EventList.Events[1].Id);
            Assert.AreEqual("timer", result.EventList.Events[2].Id);

            Assert.AreEqual(2, result.States.Count);

            Assert.AreEqual("OFF", result.States[0].Id);
            Assert.AreEqual(true, result.States[0].IsInitial);
            Assert.AreEqual(1, result.States[0].TransitionList.Transitions[0].EventIds.Count);
            Assert.AreEqual("switch_on", result.States[0].TransitionList.Transitions[0].EventIds[0]);
            Assert.AreEqual("ON", result.States[0].TransitionList.Transitions[0].Target);
            Assert.AreEqual(2, result.States[0].TransitionList.Transitions[0].ActionIds.Count);
            Assert.AreEqual("turn_on_light", result.States[0].TransitionList.Transitions[0].ActionIds[0]);
            Assert.AreEqual("set_timer_5min", result.States[0].TransitionList.Transitions[0].ActionIds[1]);


            Assert.AreEqual("ON", result.States[1].Id);
            Assert.AreEqual(false, result.States[1].IsInitial);
            Assert.AreEqual(2, result.States[1].TransitionList.Transitions[0].EventIds.Count);
            Assert.AreEqual("switch_off", result.States[1].TransitionList.Transitions[0].EventIds[0]);
            Assert.AreEqual("timer", result.States[1].TransitionList.Transitions[0].EventIds[1]);
            Assert.AreEqual("OFF", result.States[1].TransitionList.Transitions[0].Target);
            Assert.AreEqual(1, result.States[1].TransitionList.Transitions[0].ActionIds.Count);
            Assert.AreEqual("turn_off_light", result.States[1].TransitionList.Transitions[0].ActionIds[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(SyntaxErrorException))]
        public void When_InvalidInput_Assert_Exception()
        {
            var validInput = "event switch_on, switch_off, timer\n" +
                             "in initial state OFF:                           \n" +
                             "    on switch_on goto ON: turn_on_light, set_timer_5min;\n" +
                             "in state ON:                           \n" +
                             "    on switch_off, timer goto OFF: turn_off_light;\n";


            var parser = new Parser(validInput);

            var result = parser.Parse();
        }
    }
}