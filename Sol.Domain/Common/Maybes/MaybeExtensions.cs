namespace Sol.Domain.Common.Maybes
{
    public static class MaybeExtensions
    {
        public static Maybe<T> ToMaybe<T>(this T item) => new(item);
    }
}