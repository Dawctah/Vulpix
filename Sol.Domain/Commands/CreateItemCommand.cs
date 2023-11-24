using Knox.Commanding;
using Sol.Domain.Models;
using Sol.Domain.Repositories;

namespace Sol.Domain.Commands
{
    public record CreateItemCommand(string Name, HobbyType HobbyType, int Index) : Command;
    public class CreateItemCommandHandler : ICommandHandler<CreateItemCommand>
    {
        private readonly IHobbyFile hobbyFile;

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

            // hobbyFile.Insert(newItem);

            return Task.CompletedTask;
        }
    }
}
