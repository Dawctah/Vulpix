using Knox.Extensions;
using Newtonsoft.Json;
using Sol.Domain.Models;

namespace Sol.Domain.Repositories
{
    public interface IHobbyFile
    {
        IEnumerable<Item> GetAllItems(HobbyType hobbyType);

        void Update(Item item);
        void Insert(Item item);
        void Delete(Item item);
    }

    public class HobbyFile : IHobbyFile
    {
        [JsonProperty]
        private List<Item> items;

        public HobbyFile()
        {
            items = new();
        }

        public IEnumerable<Item> GetAllItems(HobbyType type) => items.Where(item => item.HobbyType == type);

        public void Update(Item item)
        {
            var updating = items.Find(i => i.Key == item.Key).Wrap().UnwrapOrTantrum()!;
            var replacementIndex = items.IndexOf(updating);

            items.RemoveAt(replacementIndex);
            items.Insert(replacementIndex, item);

            items = OrganizeItems();
        }

        public void Insert(Item item)
        {
            items.Add(item);
            items = OrganizeItems();
        }

        private List<Item> OrganizeItems() =>
            items
            .OrderBy(i => i.HobbyType)
            .ThenBy(i => i.Index)
            .ToList()
            ;

        public void Delete(Item item)
        {
            items.Remove(item);
            items = OrganizeItems();
        }
    }
}
