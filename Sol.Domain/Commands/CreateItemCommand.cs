using Knox.Commanding;
using Knox.Extensions;
using Sol.Domain.Models;
using Sol.Domain.Repositories;

namespace Sol.Domain.Commands
{
    public record CreateItemCommand(IHobbyFile HobbyFile, string Name, HobbyType HobbyType) : Command;
    public class CreateItemCommandHandler : ICommandHandler<CreateItemCommand>
    {
        public Task<bool> CanExecuteAsync(CreateItemCommand command)
        {
            throw new NotImplementedException();
        }

        public Task ExecuteAsync(CreateItemCommand command)
        {
            var index = command.HobbyFile
                .GetAllItems(command.HobbyType)
                .Where(item => item.Status == ItemStatus.NotStarted)
                .OrderBy(item => item.Index)
                .WrapLast()
                .UnwrapOrExchange(Item.Empty)
                .Index + 1;

            var newItem = new Item() with
            {
                Name = command.Name,
                HobbyType = command.HobbyType,
                Index = index
            };

            command.HobbyFile.Insert(newItem);

            return Task.CompletedTask;
        }
    }
}
