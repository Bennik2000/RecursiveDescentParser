using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace StateMachineCompiler.Test
{
    [TestClass]
    public class ScannerTest
    {
        [TestMethod]
        public void When_Valid_Input_Assert_Correctly_Scanned()
        {
            var validInput = "event switch_on, switch_off, timer;\n" +
                             "in initial state OFF:                           \n" +
                             "    on switch_on goto ON: turn_on_light, set_timer_5min;\n";

            var expectedTokens = new List<Token>()
            {
                new Token(TokenType.Event, "event"),
                new Token(TokenType.Id, "switch_on"),
                new Token(TokenType.Comma, ","),
                new Token(TokenType.Id, "switch_off"),
                new Token(TokenType.Comma, ","),
                new Token(TokenType.Id, "timer"),
                new Token(TokenType.Terminator, ";"),
                new Token(TokenType.In, "in"),
                new Token(TokenType.Initial, "initial"),
                new Token(TokenType.State, "state"),
                new Token(TokenType.Id, "OFF"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.On, "on"),
                new Token(TokenType.Id, "switch_on"),
                new Token(TokenType.Goto, "goto"),
                new Token(TokenType.Id, "ON"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.Id, "turn_on_light"),
                new Token(TokenType.Comma, ","),
                new Token(TokenType.Id, "set_timer_5min"),
                new Token(TokenType.Terminator, ";"),
            };

            var scanner = new Scanner(validInput);

            foreach (var expected in expectedTokens)
            {
                var token = scanner.GetNextToken();

                Assert.AreEqual(expected.Type, token.Type);
                Assert.AreEqual(expected.Value, token.Value);
            }

            Assert.AreEqual(TokenType._EndOfInput, scanner.GetNextToken().Type);
        }
    }
}