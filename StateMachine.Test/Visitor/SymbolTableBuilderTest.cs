using Microsoft.VisualStudio.TestTools.UnitTesting;
using StateMachineCompiler.Errors;
using StateMachineCompiler.Visitors;

namespace StateMachineCompiler.Test.Visitor
{
    [TestClass]
    public class SymbolTableBuilderTest
    {
        private SymbolTable buildSymbolTable(string input)
        {
            var parser = new Parser(input);

            var stateMachine = parser.Parse();

            var symbolTable = new SymbolTable();

            stateMachine.Accept(new SymbolTableBuilder(symbolTable));

            return symbolTable;
        }

        [TestMethod]
        public void When_CorrectInput_Assert_CorrectSymbolTable()
        {
            var symbolTable = buildSymbolTable("event switch_on, switch_off, timer;                 \n" +
                                               "in initial state OFF:                                    \n" +
                                               "  on switch_on goto ON: turn_on_light, set_timer_5min;   \n" +
                                               "in state ON:                                             \n" +
                                               "  on switch_off, timer goto OFF: turn_off_light;         \n" +
                                               "  on switch_on goto ON: turn_off_light;                  \n");

            Assert.AreEqual(3, symbolTable.Events.Count);
            Assert.IsTrue(symbolTable.Events.ContainsKey("switch_on"));
            Assert.IsTrue(symbolTable.Events.ContainsKey("switch_off"));
            Assert.IsTrue(symbolTable.Events.ContainsKey("timer"));

            Assert.AreEqual(2, symbolTable.States.Count);
            Assert.IsTrue(symbolTable.States.ContainsKey("OFF"));
            Assert.IsTrue(symbolTable.States.ContainsKey("ON"));

            Assert.AreEqual(3, symbolTable.Actions.Count);
            Assert.IsTrue(symbolTable.Actions.ContainsKey("turn_on_light"));
            Assert.IsTrue(symbolTable.Actions.ContainsKey("set_timer_5min"));
            Assert.IsTrue(symbolTable.Actions.ContainsKey("turn_off_light"));
        }

        [TestMethod]
        [ExpectedException(typeof(EventDeclaredMultipleTimesException))]
        public void When_EventDefinedTwice_Assert_ThrowsException()
        {
            buildSymbolTable("event switch_on, switch_on, timer;                  \n" +
                             "in initial state OFF:                                    \n" +
                             "  on switch_on goto ON: turn_on_light, set_timer_5min;   \n");
        }

        [TestMethod]
        [ExpectedException(typeof(StateDeclaredMultipleTimesException))]
        public void When_StateDefinedTwice_Assert_ThrowsException()
        {
            buildSymbolTable("event switch_on, switch_off, timer;                 \n" +
                             "in initial state OFF:                                    \n" +
                             "  on switch_on goto ON: turn_on_light, set_timer_5min;   \n" +
                             "in initial state OFF:                                    \n" +
                             "  on switch_on goto OFF: set_timer_5min;                 \n");
        }

        [TestMethod]
        [ExpectedException(typeof(MultipleInitialStatesException))]
        public void When_TwoInitialStates_Assert_ThrowsException()
        {
            buildSymbolTable("event switch_on, switch_off, timer;                 \n" +
                             "in initial state ON:                                     \n" +
                             "  on switch_on goto OFF: turn_on_light, set_timer_5min;  \n" +
                             "in initial state OFF:                                    \n" +
                             "  on switch_on goto ON: turn_on_light, set_timer_5min;   \n");
        }
    }
}