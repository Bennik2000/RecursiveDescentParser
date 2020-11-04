using StateMachineCompiler.Ast;
using StateMachineCompiler.Visitors;
using System.IO;
using System.Text;

namespace StateMachineCompiler.CodeGeneration
{
    public abstract class CodeGenerator
    {
        protected readonly StateMachine StateMachine;
        protected readonly SymbolTable SymbolTable;

        protected CodeGenerator(StateMachine stateMachine, SymbolTable symbolTable)
        {
            StateMachine = stateMachine;
            SymbolTable = symbolTable;
        }

        public void GenerateFile(string outputPathName)
        {
            File.WriteAllText(outputPathName, GenerateString());
        }

        public string GenerateString()
        {
            var stringBuilder = new StringBuilder();

            Generate(stringBuilder);

            return stringBuilder.ToString();
        }

        protected abstract void Generate(StringBuilder stringBuilder);
    }
}