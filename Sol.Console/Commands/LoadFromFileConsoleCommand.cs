using Sol.Console.Commanding;
using Sol.Domain.Commands;
using Sol.Domain.Common;
using Sol.Domain.Repositories;

namespace Sol.Console.Commands
{
    public class LoadFromFileConsoleCommand : IConsoleCommand
    {
        private readonly LoadFromFileCommand loadCommand;

        public LoadFromFileConsoleCommand(LoadFromFileCommand loadCommand)
        {
            this.loadCommand = loadCommand;
        }

        public string CommandDocumentation => CommandName;

        public string CommandName => "load";

        public string SuccessMessage => "Successfully loaded data from file.";

        public ISaveFile LoadedFile { get; private set; } = new SaveFile();

        public void Execute(ConsoleCommandContext context)
        {
            var loadContext = new LoadFromFileCommandContext(Data.Directory, Data.FullName(Profile.Personal));
            LoadedFile = loadCommand.Execute(loadContext);
        }
    }
}