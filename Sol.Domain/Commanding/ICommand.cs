namespace Sol.Domain.Commanding
{
    public interface ICommand<T> where T : CommandContext
    {
        public void Execute(T context);
    }

    public interface ICommand<TContext, TReturn> where TContext : CommandContext
    {
        public TReturn Execute(TContext context);
    }
}