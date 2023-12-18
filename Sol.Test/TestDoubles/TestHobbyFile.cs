using Knox.Extensions;
using Sol.Domain.Models;
using Sol.Domain.Repositories;

namespace Sol.Test.TestDoubles
{
    public class TestHobbyFile : IHobbyFile
    {
        public List<Item> Items { get; private set; }

        public List<Item> DeletedItems { get; init; } = new();
        public List<Item> InsertedItems { get; init; } = new();
        public List<Item> UpdatedItems { get; init; } = new();
        public bool Organized { get; private set; } = false;

        public string LastSelectedHobbyTypeString { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public TestHobbyFile(params Item[] items)
        {
            Items = new();
            Items.AddRange(items);
        }
        public TestHobbyFile(IEnumerable<Item> items)
        {
            Items = items.ToList();
        }

        public void Load(params Item[] items)
        {
            Items.AddRange(items);
        }

        public void Delete(Item item)
        {
            DeletedItems.Add(item);

            if (Items.Contains(item))
            {
                Items.Remove(item);
            }
        }

        public IEnumerable<Item> GetAllItems(HobbyType hobbyType) => Items;

        public void Insert(Item item)
        {
            InsertedItems.Add(item);
            Items.Add(item);
        }

        public void Update(Item item, bool organize = true)
        {
            UpdatedItems.Add(item);

            var updating = Items.Find(i => i.Key == item.Key).Wrap().UnwrapOrTantrum()!;
            var replacementIndex = Items.IndexOf(updating);

            Items.RemoveAt(replacementIndex);
            Items.Insert(replacementIndex, item);

            Organized = organize;
        }
    }
}
