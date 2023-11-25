using Newtonsoft.Json;
using Sol.Domain.Commanding;
using Sol.Domain.Common.Maybes;
using Sol.Domain.Repositories;

namespace Sol.Domain.Commands
{
    public record LegacyLoadFromFileCommandContext(string SaveDirectory, string FileName) : CommandContext();
    public class LegacyLoadFromFileCommand : ICommand<LegacyLoadFromFileCommandContext, ISaveFile>
    {
        public ISaveFile Execute(LegacyLoadFromFileCommandContext context)
        {
            try
            {
                var data = File.ReadAllText($"{context.SaveDirectory}\\{context.FileName}");
                var result = JsonConvert.DeserializeObject<SaveFile>(data).ToMaybe().GetOrElse(new())!;
                return result;
            }
            catch
            {
                return new SaveFile();
            }
        }
    }
}
