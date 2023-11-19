using Sol.Domain.Commanding;
using Sol.Domain.Models;
using Sol.Domain.Repositories;

namespace Sol.Domain.Commands
{
    public record StopReadingBookCommandContext(Book Book, ISaveFile SaveFile) : CommandContext();
    public class StopReadingBookCommand : ICommand<StopReadingBookCommandContext>
    {
        public void Execute(StopReadingBookCommandContext context)
        {
            var saveFile = context.SaveFile;
            var book = context.Book.Stop();

            saveFile.UpdateCurrentlyReading(book);
            saveFile.RemoveCurrentlyReading(book);
            saveFile.AddToBeRead(book);
        }
    }
}
