namespace Sol.Domain.Common.Maybes
{
    public class Maybe<T>
    {
        private readonly T item;

        public bool Present => item != null;

        public Maybe(T item)
        {
            this.item = item;
        }

        public T Get() => item;
        public T GetOrThrow() => item ?? throw new EmptyMaybeException("Item was empty.");
        public T GetOrThrow(string message) => item ?? throw new EmptyMaybeException(message);
        public T GetOrThrow(Exception exception) => item ?? throw exception;
        public T GetOrElse(T contingency) => item ?? contingency;
    }
}