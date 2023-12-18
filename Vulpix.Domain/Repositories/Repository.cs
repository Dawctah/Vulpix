using Knox.Extensions;
using Newtonsoft.Json;
using Vulpix.Domain.Models;

namespace Vulpix.Domain.Repositories
{
    public class Repository<T> : IRepository<T>
        where T : AggregateRoot
    {
        [JsonProperty]
        private readonly List<T> itemRepository;

        public Repository()
        {
            itemRepository = new List<T>();
        }

        public Repository(IEnumerable<T> itemRepository)
        {
            this.itemRepository = itemRepository.ToList();
        }

        public void Insert(T item) => itemRepository.Add(item);
        public void InsertMany(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                Insert(item);
            }
        }

        public IEnumerable<T> GetAll() => itemRepository;

        public void Remove(T item) => itemRepository.Remove(item);
        public void Update(T item)
        {
            var updating = itemRepository.Find(i => i.Key == item.Key).Wrap().UnwrapOrTantrum()!;
            var replacementIndex = itemRepository.IndexOf(updating);

            itemRepository.RemoveAt(replacementIndex);
            itemRepository.Insert(replacementIndex, item);
        }

        public override string ToString()
        {
            var result = string.Empty;

            foreach (var item in itemRepository)
            {
                result += $"{item}\n";
            }

            return result;
        }
    }
}
