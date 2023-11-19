using Sol.Domain.Commanding;
using Sol.Domain.Models;
using Sol.Domain.Repositories;

namespace Sol.Domain.Commands
{
    public record FinishBookCommandContext(Book Book, ISaveFile SaveFile) : CommandContext();
    public class FinishBookCommand : ICommand<FinishBookCommandContext>
    {
        public void Execute(FinishBookCommandContext context)
        {
            var saveFile = context.SaveFile;
            var book = context.Book.Finish();

            saveFile.UpdateCurrentlyReading(book);
            saveFile.RemoveCurrentlyReading(book);
            saveFile.AddFinished(book);
        }
    }
}
