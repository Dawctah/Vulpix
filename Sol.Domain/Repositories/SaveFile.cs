using Newtonsoft.Json;
using Sol.Domain.Models;

namespace Sol.Domain.Repositories
{
    public class SaveFile : ISaveFile
    {
        [JsonProperty]
        private readonly IRepository<Book> toBeRead;
        [JsonProperty]
        private readonly IRepository<Book> currentlyReading;
        [JsonProperty]
        private readonly IRepository<Book> didNotFinish;
        [JsonProperty]
        private readonly IRepository<Book> finished;

        public SaveFile()
        {
            toBeRead = new Repository<Book>();
            currentlyReading = new Repository<Book>();
            didNotFinish = new Repository<Book>();
            finished = new Repository<Book>();
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
        public IEnumerable<Book> GetFinished() => finished.GetAll().OrderByDescending(b => b.FinishDate);

        public void UpdateToBeRead(Book book) => toBeRead.Update(book);

        public void UpdateCurrentlyReading(Book book) => currentlyReading.Update(book);

        public void UpdateDidNotFinish(Book book) => didNotFinish.Update(book);

        public void UpdateFinished(Book book) => finished.Update(book);
    }
}
