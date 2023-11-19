namespace Sol.Console.Commanding
{
    public interface IConsoleCommand
    {
        string CommandDocumentation { get; }
        string CommandName { get; }
        string SuccessMessage { get; }

        void Execute(ConsoleCommandContext context);
    }
}
