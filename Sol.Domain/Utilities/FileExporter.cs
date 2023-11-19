using System.Text;

namespace Sol.Domain.Utilities
{
    public class FileExporter : IExporter
    {
        public void Export(string directory, string fileName, Func<string> stringResult)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using var stream = File.Create($"{directory}\\{fileName}");

            stream.Write(new UTF8Encoding(true).GetBytes(stringResult()));
        }
    }
}
