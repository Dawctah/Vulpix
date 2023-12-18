using Knox.Commanding;
using Knox.Extensions;
using Sol.Domain.Models;
using Sol.Domain.Repositories;

namespace Sol.Domain.Commands
{
    public record ChangeItemStatusCommand(IHobbyFile HobbyFile, Item Item, ItemStatus Status) : Command;
    public class ChangeItemStatusCommandHandler : ICommandHandler<ChangeItemStatusCommand>
    {
        public Task<bool> CanExecuteAsync(ChangeItemStatusCommand command)
        {
            throw new NotImplementedException();
        }

        public Task ExecuteAsync(ChangeItemStatusCommand command)
        {
            var saveFile = command.HobbyFile;
            var index = saveFile
                .GetAllItems(command.Item.HobbyType)
                .Where(item => item.Status == command.Status)
                .OrderBy(item => item.Index)
                .WrapLast()
                .UnwrapOrExchange(Item.Empty)
                .Index
                + 1
                ;

            var item = command.Item
                .ChangeStatus(command.Status)
                .UpdateIndex(index);

            saveFile.Update(item);

            return Task.CompletedTask;
        }
    }
}
