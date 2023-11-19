using Sol.Domain.Commanding;
using Sol.Domain.Common.Maybes;
using Sol.Domain.Models;
using Sol.Domain.Repositories;

namespace Sol.Domain.Commands
{
    public record SwapBookOrderCommandContext(Book BookOne, Book BookTwo, ISaveFile SaveFile) : CommandContext();
    public class SwapBookOrderCommand : ICommand<SwapBookOrderCommandContext>
    {
        public void Execute(SwapBookOrderCommandContext context)
        {
            var (book1, book2, saveFile) = context;

            var newBook1 = saveFile.GetToBeRead().Where(b => b.Key == book1.Key).First().ToMaybe().GetOrThrow("Selected book was not found.").Update(book2);
            var newBook2 = saveFile.GetToBeRead().Where(b => b.Key == book2.Key).First().ToMaybe().GetOrThrow("Target index was not found.").Update(book1);

            if (newBook1.Valid() && newBook2.Valid())
            {
                saveFile.UpdateToBeRead(newBook1);
                saveFile.UpdateToBeRead(newBook2);
            }
        }
    }
}
