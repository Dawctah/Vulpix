using Knox.Commanding;
using Vulpix.Domain.Models;
using Vulpix.Domain.Repositories;

namespace Vulpix.Domain.Commands
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
