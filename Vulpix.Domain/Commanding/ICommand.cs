namespace Vulpix.Domain.Commanding
{
    [Obsolete("ICommand<T> is obsolete, use Knox.Commanding.ICommandHandler<T> instead.")]
    public interface ICommand<T> where T : CommandContext
    {
        public void Execute(T context);
    }

    [Obsolete("ICommand<TContext, TReturn> is no longer used.")]
    public interface ICommand<TContext, TReturn> where TContext : CommandContext
    {
        public TReturn Execute(TContext context);
    }
}