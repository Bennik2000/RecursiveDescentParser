namespace StateMachineCompiler
{
    public enum TokenType
    {
        Event,
        Id,
        Terminator,
        Comma,
        In,
        On,
        Initial,
        State,
        Colon,
        Goto,
        Blank,
        _Invalid,
        _EndOfInput,
    }
}