using Knox.Commanding;
using Sol.Domain.Models;
using Sol.Domain.Repositories;

namespace Sol.Domain.Commands
{
    public record CompleteItemCommand(IHobbyFile HobbyFile, Item Item) : Command;
    public class CompleteItemCommandHandler : ICommandHandler<CompleteItemCommand>
    {
        public Task<bool> CanExecuteAsync(CompleteItemCommand command)
        {
            throw new NotImplementedException();
        }

        public Task ExecuteAsync(CompleteItemCommand command)
        {
            var item = command.Item
                .ChangeStatus(ItemStatus.Complete);

            command.HobbyFile.Update(item);

            // Loop through all items and order by date.
            var completedItems = command.HobbyFile.GetAllItems(item.HobbyType).Where(i => i.Status == ItemStatus.Complete).OrderByDescending(i => i.CompletionDate);
            var index = 0;

            foreach (var completedItem in completedItems)
            {
                index++;
                command.HobbyFile.Update(completedItem with { Index = index });
            }

            return Task.CompletedTask;
        }
    }
}
