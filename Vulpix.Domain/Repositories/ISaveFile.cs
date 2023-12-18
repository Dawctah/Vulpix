using Vulpix.Domain.Models;

namespace Vulpix.Domain.Repositories
{
    public interface ISaveFile
    {
        void AddToBeRead(Book book);
        void AddCurrentlyReading(Book book);
        void AddDidNotFinish(Book book);
        void AddFinished(Book book);

        void RemoveToBeRead(Book book);
        void RemoveCurrentlyReading(Book book);
        void RemoveDidNotFinish(Book book);
        void RemoveFinished(Book book);

        void UpdateToBeRead(Book book);
        void UpdateCurrentlyReading(Book book);
        void UpdateDidNotFinish(Book book);
        void UpdateFinished(Book book);

        IEnumerable<Book> GetToBeRead();
        IEnumerable<Book> GetCurrentlyReading();
        IEnumerable<Book> GetDidNotFinish();
        IEnumerable<Book> GetFinished();
    }

    public enum Profile
    {
        Personal,
        BookClub,
    }
}
