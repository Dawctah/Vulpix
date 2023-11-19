using Sol.Domain.Common.Maybes;
using Sol.Domain.Models;
using Sol.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            foreach(T item in items)
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

            var updating = SavedItems.Find(i => i.Key == item.Key).ToMaybe().GetOrThrow()!;
            var replacementIndex = SavedItems.IndexOf(updating);

            SavedItems.RemoveAt(replacementIndex);
            SavedItems.Insert(replacementIndex, item);
        }
    }
}
