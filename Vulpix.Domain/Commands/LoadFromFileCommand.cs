using Knox.Extensions;
using Newtonsoft.Json;
using Vulpix.Domain.Repositories;

/// <summary>
/// This command loads the file, so it must return it, making it a bit of a strange command.
/// </summary>
namespace Vulpix.Domain.Commands
{
    public record LoadFromFileCommand(string SaveDirectory, string FullName)
    {
        public static HobbyFile Execute(LoadFromFileCommand command)
        {
            try
            {
                var data = File.ReadAllText($"{command.SaveDirectory}\\{command.FullName}");
                var result = JsonConvert.DeserializeObject<HobbyFile>(data).Wrap().UnwrapOrExchange(new())!;

                return result;
            }
            catch
            {
                return new HobbyFile();
            }
        }
    }
}