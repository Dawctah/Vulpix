using Knox.ConsoleCommanding;
using Sol.Domain.Commands;
using Sol.Domain.Common;
using Sol.Domain.Models;
using Sol.Domain.Repositories;

namespace Sol.Console.Commands
{
    public class ConvertLegacyToModernConsoleCommand : IConsoleCommand
    {
        private readonly LegacyLoadFromFileCommand legacyLoadCommand;
        private readonly SaveHobbiesToFileCommandHandler saveHobbiesToFileCommand;

        public ConvertLegacyToModernConsoleCommand(LegacyLoadFromFileCommand legacyLoadCommand, SaveHobbiesToFileCommandHandler saveHobbiesToFileCommand)
        {
            this.legacyLoadCommand = legacyLoadCommand;
            this.saveHobbiesToFileCommand = saveHobbiesToFileCommand;
        }

        public string CommandDocumentation => CommandName;

        public string CommandName => "convert-legacy";

        public async Task ExecuteAsync(ConsoleCommandContext context)
        {
            // Get old file data.
            var legacyPersonal = legacyLoadCommand.Execute(new(Data.Directory, Data.FullName(Profile.Personal)));

            var newFile = new HobbyFile();

            // Convert to new file data.
            ConvertBooks(newFile, legacyPersonal.GetToBeRead(), HobbyType.Reading, ItemStatus.NotStarted);
            ConvertBooks(newFile, legacyPersonal.GetCurrentlyReading(), HobbyType.Reading, ItemStatus.InProgress);
            ConvertBooks(newFile, legacyPersonal.GetFinished(), HobbyType.Reading, ItemStatus.Complete);

            // Get old book club data.
            var legacyBookClub = legacyLoadCommand.Execute(new(Data.Directory, Data.FullName(Profile.BookClub)));

            ConvertBooks(newFile, legacyBookClub.GetToBeRead(), HobbyType.BookClub, ItemStatus.NotStarted);
            ConvertBooks(newFile, legacyBookClub.GetCurrentlyReading(), HobbyType.BookClub, ItemStatus.InProgress);
            ConvertBooks(newFile, legacyBookClub.GetFinished(), HobbyType.BookClub, ItemStatus.Complete);

            // Save.
            await saveHobbiesToFileCommand.ExecuteAsync(new(newFile, Data.Directory, Data.FullName()));
        }

        public string SuccessMessage(ConsoleCommandContext context)
        {
            throw new NotImplementedException();
        }

        private void ConvertBooks(HobbyFile hobbyFile, IEnumerable<Book> books, HobbyType hobbyType, ItemStatus itemStatus)
        {
            var index = 0;
            foreach (var book in books)
            {
                index++;
                var item = Item.Empty with
                {
                    Name = book.Title,
                    StartedDate = book.StartDate,
                    CompletionDate = book.FinishDate,
                    Key = book.Key,
                    HobbyType = hobbyType,
                    Index = index,
                    Status = itemStatus,
                };

                hobbyFile.Insert(item);
            }
        }
    }
}
