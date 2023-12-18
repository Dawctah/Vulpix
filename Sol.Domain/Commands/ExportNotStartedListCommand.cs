using Knox.Commanding;
using Sol.Domain.Utilities;
using System.Text;

namespace Sol.Domain.Commands
{
    public record ExportNotStartedListCommand(string SaveDirectory, string FileName, IEnumerable<string> Titles) : Command;
    public class ExportNotStartedListCommandHandler : ICommandHandler<ExportNotStartedListCommand>
    {
        private readonly IExporter exporter;

        public ExportNotStartedListCommandHandler(IExporter exporter)
        {
            this.exporter = exporter;
        }

        public Task<bool> CanExecuteAsync(ExportNotStartedListCommand command)
        {
            throw new NotImplementedException();
        }

        public Task ExecuteAsync(ExportNotStartedListCommand command)
        {
            exporter.Export(command.SaveDirectory, command.FileName, () =>
            {
                var result = new StringBuilder();
                foreach (var title in command.Titles)
                {
                    result.Append($"{title}\n");
                }

                return result.ToString();
            });

            return Task.CompletedTask;
        }
    }
}