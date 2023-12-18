using Vulpix.Domain.Models;
using Vulpix.Domain.Repositories;

namespace Vulpix.Test.TestDoubles
{
    public class TestSaveFile : ISaveFile
    {
        public TestRepository<Book> toBeRead;
        public TestRepository<Book> currentlyReading;
        public TestRepository<Book> didNotFinish;
        public TestRepository<Book> finished;

        public TestSaveFile()
        {
            toBeRead = new();
            currentlyReading = new();
            didNotFinish = new();
            finished = new();
        }

        public void AddToBeRead(Book book) => toBeRead.Insert(book);
        public void AddCurrentlyReading(Book book) => currentlyReading.Insert(book);
        public void AddDidNotFinish(Book book) => didNotFinish.Insert(book);
        public void AddFinished(Book book) => finished.Insert(book);

        public void RemoveToBeRead(Book book) => toBeRead.Remove(book);
        public void RemoveCurrentlyReading(Book book) => currentlyReading.Remove(book);
        public void RemoveDidNotFinish(Book book) => didNotFinish.Remove(book);
        public void RemoveFinished(Book book) => finished.Remove(book);

        public IEnumerable<Book> GetToBeRead() => toBeRead.GetAll();
        public IEnumerable<Book> GetCurrentlyReading() => currentlyReading.GetAll();
        public IEnumerable<Book> GetDidNotFinish() => didNotFinish.GetAll();
        public IEnumerable<Book> GetFinished() => finished.GetAll();

        public void UpdateToBeRead(Book book) => toBeRead.Update(book);
        public void UpdateCurrentlyReading(Book book) => currentlyReading.Update(book);
        public void UpdateDidNotFinish(Book book) => didNotFinish.Update(book);
        public void UpdateFinished(Book book) => finished.Update(book);
    }
}
