using Sol.Console.Commanding;
using Sol.Domain.Commanding;
using Sol.Domain.Commands;
using Sol.Domain.Common;
using Sol.Domain.Repositories;

namespace Sol.Console.Commands
{
    public class SaveToFileConsoleCommand : IConsoleCommand
    {
        private readonly ISaveFile saveFile;
        private readonly ICommand<SaveToFileCommandContext> saveCommand;

        public string CommandDocumentation => CommandName;

        public string CommandName => "save";

        public string SuccessMessage => "Successfully saved to file";

        public SaveToFileConsoleCommand(ISaveFile saveFile, ICommand<SaveToFileCommandContext> saveCommand)
        {
            this.saveFile = saveFile;
            this.saveCommand = saveCommand;
        }

        public void Execute(ConsoleCommandContext context)
        {
            var saveContext = new SaveToFileCommandContext(Data.Directory, Data.FullName(Profile.Personal), saveFile);
            saveCommand.Execute(saveContext);
        }
    }
}
