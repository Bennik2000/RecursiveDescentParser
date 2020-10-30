using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StateMachine.Test
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

            Assert.AreEqual(3, result.EventDeclaration.EventIds.Count);
            Assert.AreEqual("switch_on", result.EventDeclaration.EventIds[0]);
            Assert.AreEqual("switch_off", result.EventDeclaration.EventIds[1]);
            Assert.AreEqual("timer", result.EventDeclaration.EventIds[2]);

            Assert.AreEqual(2, result.StateDeclarations.Count);

            Assert.AreEqual("OFF", result.StateDeclarations[0].Id);
            Assert.AreEqual(true, result.StateDeclarations[0].IsInitial);
            Assert.AreEqual(1, result.StateDeclarations[0].Transition.Triggers.Count);
            Assert.AreEqual("switch_on", result.StateDeclarations[0].Transition.Triggers[0]);
            Assert.AreEqual("ON", result.StateDeclarations[0].Transition.Target);
            Assert.AreEqual(2, result.StateDeclarations[0].Transition.Events.Count);
            Assert.AreEqual("turn_on_light", result.StateDeclarations[0].Transition.Events[0]);
            Assert.AreEqual("set_timer_5min", result.StateDeclarations[0].Transition.Events[1]);


            Assert.AreEqual("ON", result.StateDeclarations[1].Id);
            Assert.AreEqual(false, result.StateDeclarations[1].IsInitial);
            Assert.AreEqual(2, result.StateDeclarations[1].Transition.Triggers.Count);
            Assert.AreEqual("switch_off", result.StateDeclarations[1].Transition.Triggers[0]);
            Assert.AreEqual("timer", result.StateDeclarations[1].Transition.Triggers[1]);
            Assert.AreEqual("OFF", result.StateDeclarations[1].Transition.Target);
            Assert.AreEqual(1, result.StateDeclarations[1].Transition.Events.Count);
            Assert.AreEqual("turn_off_light", result.StateDeclarations[1].Transition.Events[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(SyntaxErrorException))]
        public void When_InvalidInput_Assert_Exception()
        {
            var validInput = "event switch_on, switch_off, timer;\n" +
                             "in initial state OFF:                           \n" +
                             "    on switch_on goto ON: turn_on_light, set_timer_5min;\n" +
                             "in state ON:                           \n" +
                             "    on switch_off, timer goto OFF: turn_off_light;\n";


            var parser = new Parser(validInput);

            var result = parser.Parse();
        }
    }
}