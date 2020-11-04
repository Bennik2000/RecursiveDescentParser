using System;
using System.Diagnostics;
using StateMachineCompiler.Visitors;

namespace StateMachineCompiler
{
    class Program
    {
        static void Main(string[] args)
        {
			/**
             *
             *
             * Grammatik (Name der Nichtterminale mitlerweile nicht mehr konsistent mit code!):
			 *
			G={N,T, P, s}

            N= {
	            EVENT_DECLARATION,
	            STATE_DECLARATION,
	            STATE_DECLARATIONS,
	            STATE_TRANSITION,
	            STATE_TRANSITIONS,
	            ID_LIST,
	            STATE_MACHINE,
            }

            T= {
	            EVENT: "event",
	            ID: "[a-zA-Z_]+[0-9a-zA-Z_]*,
	            TERMINATOR: ";",
	            COMMA: ",",
	            IN: "in",
	            ON: "on",
	            INITIAL: "initial",
	            STATE: "state",
	            COLON: ":",
	            GOTO: "goto",
            }

            P = {
	            STATE_MACHINE -> EVENT_DECLARATION STATE_DECLARATIONS,
	            
	            EVENT_DECLARATION -> EVENT ID_LIST TERMINATOR
	            
	            STATE_DECLARATIONS -> STATE_DECLARATION STATE_DECLARATIONS |STATE_DECLARATION
	            STATE_DECLARATION -> IN OPTIONAL_INITIAL STATE ID COLON STATE_TRANSITIONS
	            
	            STATE_TRANSITIONS -> STATE_TRANSITION | STATE_TRANSITIONS | STATE_TRANSITION
	            STATE_TRANSITION -> ON ID, GOTO COLON ID_LIST TERMINATOR
	            
	            ID_LIST -> ID COMMA ID_LIST | ID
	            
	            OPTIONAL_INITIAL -> INITIAL | ε
            }

            S = STATE_MACHINE


             *
             */

			var validInput = "event switch_on, switch_off, timer;                      \n" +
                             "in initial state OFF:                                    \n" +
                             "  on switch_on goto ON: turn_on_light, set_timer_5min;   \n" +
                             "in state ON:                                             \n" +
                             "  on switch_off, timer goto OFF: turn_off_light;         \n" +
                             "  on switch_on goto bar: turn_off_light;                 \n" +
                             "in state bar:                                            \n" +
                             "  on timer goto bar: turn_off_light;                     \n";

			var parser = new Parser(validInput);

            var stateMachine = parser.Parse();

			var symbolTable = new SymbolTable();

            stateMachine.Accept(new SymbolTableBuilder(symbolTable));
            stateMachine.Accept(new EventResolver(symbolTable));
            stateMachine.Accept(new StateResolver(symbolTable));
			stateMachine.Accept(new ActionResolver(symbolTable));
            stateMachine.Accept(new TransitionSemanticChecker());


			// If the program reaches the end the input is valid
		}
    }
}
