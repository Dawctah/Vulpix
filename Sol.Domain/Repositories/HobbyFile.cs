using Knox.Extensions;
using Newtonsoft.Json;
using Sol.Domain.Models;

namespace Sol.Domain.Repositories
{
    public class HobbyFile : IHobbyFile
    {
        [JsonProperty]
        private List<Item> items;

        public string LastSelectedHobbyTypeString { get; set; } = HobbyType.Miscellaneous.ToString();

        public HobbyFile()
        {
            items = new();
        }

        public IEnumerable<Item> GetAllItems(HobbyType type) => items.Where(item => item.HobbyType == type);

        public void Update(Item item, bool organize = true)
        {
            var updating = items.Find(i => i.Key == item.Key).Wrap().UnwrapOrTantrum()!;
            var replacementIndex = items.IndexOf(updating);

            items.RemoveAt(replacementIndex);
            items.Insert(replacementIndex, item);

            if (organize)
            {
                items = OrganizeItems(item.Status, item.HobbyType);
            }
        }

        public void Insert(Item item)
        {
            items.Add(item);
            items = OrganizeItems(item.Status, item.HobbyType);
        }

        private List<Item> OrganizeItems(ItemStatus itemStatus, HobbyType hobbyType)
        {
            var adjustedList = items
                .Where(item => item.Status == itemStatus && item.HobbyType == hobbyType)
                .OrderBy(item => item.Index)
                .ToList()
                ;

            var result = new List<Item>();
            for (var index = 0; index < adjustedList.Count; index++)
            {
                result.Add(adjustedList[index] with
                {
                    Index = index + 1,
                });
            }

            // Whatever items are missing need to be placed back.
            var missing = items.Where(item => item.Status != itemStatus || item.HobbyType != hobbyType);

            return result.Union(missing).ToList();
        }

        public void Delete(Item item)
        {
            items.Remove(item);
            items = OrganizeItems(item.Status, item.HobbyType);
        }
    }
}
