using Sol.Domain.Commanding;
using Sol.Domain.Models;
using Sol.Domain.Repositories;

namespace Sol.Domain.Commands
{
    public record StartReadingBookCommandContext(Book Book, ISaveFile SaveFile) : CommandContext();
    public class StartReadingBookCommand : ICommand<StartReadingBookCommandContext>
    {
        public void Execute(StartReadingBookCommandContext context)
        {
            var saveFile = context.SaveFile;
            var book = context.Book.Start();
            saveFile.UpdateToBeRead(book);

            saveFile.RemoveToBeRead(book);
            saveFile.AddCurrentlyReading(book);
        }
    }
}
