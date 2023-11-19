using Sol.Domain.Commanding;
using Sol.Domain.Models;
using Sol.Domain.Repositories;

namespace Sol.Domain.Commands
{
    public record DoNotFinishBookCommandContext(Book Book, ISaveFile SaveFile) : CommandContext();
    public class DoNotFinishBookCommand : ICommand<DoNotFinishBookCommandContext>
    {
        public void Execute(DoNotFinishBookCommandContext context)
        {
            var saveFile = context.SaveFile;
            var book = context.Book.Finish();

            saveFile.UpdateCurrentlyReading(book);
            saveFile.RemoveCurrentlyReading(book);
            saveFile.AddDidNotFinish(book);
        }
    }
}