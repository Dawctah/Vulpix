using Sol.Domain.Commanding;
using Sol.Domain.Utilities;
using System.Text;

namespace Sol.Domain.Commands
{
    public record ExportTbrToFileCommandContext(string SaveDirectory, string FileName, IEnumerable<string> Titles) : CommandContext();
    public class ExportTbrToFileCommand : ICommand<ExportTbrToFileCommandContext>
    {
        private readonly IExporter exporter;

        public ExportTbrToFileCommand(IExporter exporter)
        {
            this.exporter = exporter;
        }

        public void Execute(ExportTbrToFileCommandContext context)
        {
            exporter.Export(context.SaveDirectory, context.FileName, () =>
            {
                var result = new StringBuilder();
                foreach (var title in context.Titles)
                {
                    result.Append($"{title}\n");
                }

                return result.ToString();
            });
        }
    }
}
