namespace Vulpix.Domain.Utilities
{
    public interface IExporter
    {
        public void Export(string directory, string fileName, Func<string> serializeFunc);
    }
}
