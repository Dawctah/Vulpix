using Knox.Extensions;
using Sol.Domain.Models;
using Sol.Domain.Repositories;

namespace Sol.Test.TestDoubles
{
    public class TestRepository<T> : IRepository<T>
        where T : AggregateRoot
    {
        public int Calls { get; private set; } = 0;
        public List<T> SavedItems { get; set; } = new List<T>();

        public IEnumerable<T> GetAll()
        {
            Calls++;
            return SavedItems;
        }

        public void Insert(T item)
        {
            Calls++;
            SavedItems.Add(item);
        }

        public void InsertMany(IEnumerable<T> items)
        {
            Calls++;
            foreach (var item in items)
            {
                SavedItems.Add(item);
            }
        }

        public void Remove(T item)
        {
            Calls++;
            SavedItems.Remove(item);
        }

        public void Update(T item)
        {
            Calls++;

            var updating = SavedItems.Find(i => i.Key == item.Key).Wrap().UnwrapOrTantrum()!;
            var replacementIndex = SavedItems.IndexOf(updating);

            SavedItems.RemoveAt(replacementIndex);
            SavedItems.Insert(replacementIndex, item);
        }
    }
}
