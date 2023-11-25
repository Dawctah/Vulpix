using Knox.Commanding;
using Sol.Domain.Models;
using Sol.Domain.Repositories;

namespace Sol.Domain.Commands
{
    public record CreateItemCommand(IHobbyFile HobbyFile, string Name, HobbyType HobbyType, int Index) : Command;
    public class CreateItemCommandHandler : ICommandHandler<CreateItemCommand>
    {
        public Task<bool> CanExecuteAsync(CreateItemCommand command)
        {
            throw new NotImplementedException();
        }

        public Task ExecuteAsync(CreateItemCommand command)
        {
            var newItem = new Item() with
            {
                Name = command.Name,
                HobbyType = command.HobbyType,
                Index = command.Index
            };

            command.HobbyFile.Insert(newItem);

            return Task.CompletedTask;
        }
    }
}
