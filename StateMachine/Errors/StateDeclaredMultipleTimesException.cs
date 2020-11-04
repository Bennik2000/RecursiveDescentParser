namespace StateMachineCompiler.Errors
{
    public class StateDeclaredMultipleTimesException : SemanticErrorException
    {
        public readonly string State;

        public StateDeclaredMultipleTimesException(string stateId)
        {
            State = stateId;
        }
    }
}