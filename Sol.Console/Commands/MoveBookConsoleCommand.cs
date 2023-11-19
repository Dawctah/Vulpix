using Sol.Console.Commanding;

namespace Sol.Console.Commands
{
    public class MoveBookConsoleCommand : IConsoleCommand
    {
        public string CommandDocumentation => $"{CommandName} [TARGET REPOSITORY INITIALS] [TITLE]";

        public string CommandName => "move-book";

        public string SuccessMessage => "Moved book to target repository";

        public void Execute(ConsoleCommandContext context)
        {

        }
    }
}