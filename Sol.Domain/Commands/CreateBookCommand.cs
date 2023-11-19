using Sol.Domain.Commanding;
using Sol.Domain.Models;
using Sol.Domain.Repositories;

namespace Sol.Domain.Commands
{
    public record CreateBookCommandContext(string Title, ISaveFile SaveFile) : CommandContext();
    public class CreateBookCommand : ICommand<CreateBookCommandContext>
    {
        public void Execute(CreateBookCommandContext context)
        {
            var saveFile = context.SaveFile;
            var book = Book.Create(context.Title);

            if (book.Valid())
            {
                saveFile.AddToBeRead(book);
            }
        }
    }
}