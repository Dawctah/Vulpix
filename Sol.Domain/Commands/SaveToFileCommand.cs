using Newtonsoft.Json;
using Sol.Domain.Commanding;
using Sol.Domain.Repositories;
using Sol.Domain.Utilities;

namespace Sol.Domain.Commands
{
    public record SaveToFileCommandContext(string SaveDirectory, string FileName, ISaveFile SaveFile) : CommandContext();
    public class SaveToFileCommand : ICommand<SaveToFileCommandContext>
    {
        private readonly IExporter exporter;

        public SaveToFileCommand(IExporter exporter)
        {
            this.exporter = exporter;
        }

        public void Execute(SaveToFileCommandContext context)
        {
            exporter.Export(context.SaveDirectory, context.FileName, () => JsonConvert.SerializeObject(context.SaveFile));
        }
    }
}