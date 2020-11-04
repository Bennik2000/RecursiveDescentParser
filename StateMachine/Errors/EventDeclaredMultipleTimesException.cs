namespace StateMachineCompiler.Errors
{
    public class EventDeclaredMultipleTimesException : SemanticErrorException
    {
        public readonly string Event;

        public EventDeclaredMultipleTimesException(string eventId)
        {
            Event = eventId;
        }
    }
}