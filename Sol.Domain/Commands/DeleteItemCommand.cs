using Knox.Commanding;
using Sol.Domain.Models;
using Sol.Domain.Repositories;

namespace Sol.Domain.Commands
{
    public record DeleteItemCommand(IHobbyFile HobbyFile, Item Item) : Command;
    public class DeleteItemCommandHandler : ICommandHandler<DeleteItemCommand>
    {
        public Task<bool> CanExecuteAsync(DeleteItemCommand command)
        {
            throw new NotImplementedException();
        }

        public Task ExecuteAsync(DeleteItemCommand command)
        {
            command.HobbyFile.Delete(command.Item);

            return Task.CompletedTask;
        }
    }
}
