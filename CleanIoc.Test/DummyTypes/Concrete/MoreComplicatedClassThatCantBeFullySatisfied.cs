namespace CleanIoc.Core.Test.DummyTypes.Concrete
{
    using CleanIoc.Core.Test.DummyTypes.Interfaces;

    public class MoreComplicatedClassThatCantBeFullySatisfied : IThirdInterface
    {
        public MoreComplicatedClassThatCantBeFullySatisfied(ISecondInterface paramOne, IFourthInterfaceActuallyDoesntHaveAnyDerivedClasses two)
        {
            ParamOne = paramOne;
            Two = two;
        }

        public ISecondInterface ParamOne { get; }

        public IFourthInterfaceActuallyDoesntHaveAnyDerivedClasses Two { get; }
    }
}
