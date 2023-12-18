using Knox.Commanding;
using Knox.Extensions;
using Vulpix.Domain.Models;
using Vulpix.Domain.Repositories;

namespace Vulpix.Domain.Commands
{
    /// <summary>
    /// </summary>
    /// <param name="Direction">-1 for up, 1 for down.</param>
    public record BumpItemCommand(IHobbyFile HobbyFile, Item Item, int Direction) : Command;
    public class BumpItemCommandHandler : ICommandHandler<BumpItemCommand>
    {
        public Task<bool> CanExecuteAsync(BumpItemCommand command)
        {
            throw new NotImplementedException();
        }

        public Task ExecuteAsync(BumpItemCommand command)
        {
            var item1 = command.Item;

            // Find the second item.
            var item2 = command.HobbyFile
                .GetAllItems(item1.HobbyType)
                .Where(item => item.Status == item1.Status && item.Index == item1.Index + command.Direction)
                .WrapFirst()
                .UnwrapOrExchange(Item.Empty)
                ;

            item1 = item1 with
            {
                Index = item2.Index
            };

            item2 = item2 with
            {
                Index = command.Item.Index
            };

            command.HobbyFile.Update(item1, false);
            command.HobbyFile.Update(item2);

            return Task.CompletedTask;
        }
    }
}
