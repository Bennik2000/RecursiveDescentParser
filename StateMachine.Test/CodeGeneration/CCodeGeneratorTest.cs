using Microsoft.VisualStudio.TestTools.UnitTesting;
using StateMachineCompiler.CodeGeneration;
using StateMachineCompiler.Visitors;

namespace StateMachineCompiler.Test.CodeGeneration
{
    [TestClass]
    public class CCodeGeneratorTest
    {
        [TestMethod]
        public void When_CorrectInput_Assert_CorrectCodeGenerated()
        {
            var input =
                "event switch_on, switch_off, timer;                      \n" +
                "in initial state OFF:                                    \n" +
                "  on switch_on goto ON: turn_on_light, set_timer_5min;   \n" +
                "                                                         \n" +
                "in state ON:                                             \n" +
                "  on switch_off, timer goto OFF: turn_off_light;         \n";

            var parser = new Parser(input);

            var stateMachine = parser.Parse();


            var symbolTable = new SymbolTable();

            stateMachine.Accept(new SymbolTableBuilder(symbolTable));
            stateMachine.Accept(new EventResolver(symbolTable));
            stateMachine.Accept(new StateResolver(symbolTable));
            stateMachine.Accept(new ActionResolver(symbolTable));
            stateMachine.Accept(new TransitionSemanticChecker());


            var cCode = new CCodeGenerator(stateMachine, symbolTable).GenerateString();


            var expected = "enum STATE { OFF, ON } the_state = OFF;\n" +
                           "enum EVENT { switch_on, switch_off, timer }\n" +
                           "\n" +
                           "void handleEvent(enum EVENT ev) {\n" +
                           "  switch (the_state) {\n" +
                           "  case OFF:\n" +
                           "    switch (ev) {\n" +
                           "    case switch_on:\n" +
                           "      the_state = ON;\n" +
                           "      turn_on_light();\n" +
                           "      set_timer_5min();\n" +
                           "      break;\n" +
                           "    default:\n" +
                           "      error(the_state, ev);\n" +
                           "    }\n" +
                           "    break;\n" +
                           "  case ON:\n" +
                           "    switch (ev) {\n" +
                           "    case switch_off:\n" +
                           "    case timer:\n" +
                           "      the_state = OFF;\n" +
                           "      turn_off_light();\n" +
                           "      break;\n" +
                           "    default:\n" +
                           "      error(the_state, ev);\n" +
                           "    }\n" +
                           "    break;\n" +
                           "  }\n" +
                           "}\n" +
                           "\n";

            var expectedLines = expected.Split('\n');
            var actualLines = cCode.Split('\n');

            Assert.AreEqual(expectedLines.Length, actualLines.Length);

            for (int i = 0; i < expectedLines.Length; i++)
            {
                Assert.AreEqual(expectedLines[i], actualLines[i].TrimEnd());
            }
        }
    }
}
