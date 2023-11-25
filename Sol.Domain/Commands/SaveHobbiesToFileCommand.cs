using Knox.Commanding;
using Newtonsoft.Json;
using Sol.Domain.Repositories;
using Sol.Domain.Utilities;

namespace Sol.Domain.Commands
{
    public record SaveHobbiesToFileCommand(IHobbyFile HobbyFile, string SaveDirectory, string FullName) : Command;

    public class SaveHobbiesToFileCommandHandler : ICommandHandler<SaveHobbiesToFileCommand>
    {
        private readonly IExporter exporter;

        public SaveHobbiesToFileCommandHandler(IExporter exporter)
        {
            this.exporter = exporter;
        }

        public Task<bool> CanExecuteAsync(SaveHobbiesToFileCommand command) => Task.FromResult(true);

        public Task ExecuteAsync(SaveHobbiesToFileCommand command)
        {
            exporter.Export(command.SaveDirectory, command.FullName, () => JsonConvert.SerializeObject(command.HobbyFile));

            return Task.CompletedTask;
        }
    }
}
