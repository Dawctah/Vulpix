namespace Sol.Domain.Common.Maybes
{
    public class EmptyMaybeException : Exception
    {
        public EmptyMaybeException(string message) : base(message)
        { }
    }
}