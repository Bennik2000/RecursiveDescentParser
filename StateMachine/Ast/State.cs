using StateMachineCompiler.Visitors;

namespace StateMachineCompiler.Ast
{
    public class State : AstNode
    {
        public State(string id, bool isInitial, StateTransitionList transitionList)
        {
            Id = id;
            IsInitial = isInitial;
            TransitionList = transitionList;
        }

        public bool IsInitial { get; }
        public string Id { get; }
        public StateTransitionList TransitionList { get; }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);

            TransitionList.Accept(visitor);
        }
    }
}