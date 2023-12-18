using Sol.Domain.Models;

namespace Sol.Test.Helpers
{
    public static class ItemHelper
    {
        public static Item GenerateItem(string name = "GeneratedItem", HobbyType hobbyType = HobbyType.Miscellaneous, int index = 1, ItemStatus itemStatus = ItemStatus.NotStarted) => Item.Empty with
        {
            Name = name,
            HobbyType = hobbyType,
            Index = index,
            Key = Guid.NewGuid(),
            Status = itemStatus,
        };

        public static List<Item> GenerateMultiple(int amountToGenerate, HobbyType hobbyType = HobbyType.Miscellaneous, ItemStatus itemStatus = ItemStatus.NotStarted)
        {
            var result = new List<Item>();

            for (var index = 0; index < amountToGenerate; index++)
            {
                result.Add(GenerateItem(GenerateName(), hobbyType, index + 1, itemStatus));
            }

            return result;
        }

        private static string GenerateName()
        {
            var names = new List<string>()
            {
                "The Final Empire",
                "Tress of the Emerald Sea",
                "The Way of Kings",
                "Babel",
                "Skyward",
                "Lies of P",
                "Kena: Bridge of Spirits",
                "Spider-Man 2",
                "Into the Spider-verse",
                "Community",
                "The Legend of Vox Machina",
                "Scott Pilgrim Takes Off",
                "The Mummy",
                "Moon Dust",
                "Faetography",
                "The Forgotten Kingdom of Ash",
                "Video Game Enthusiasts"
            };

            var random = new Random();

            return names[random.Next(names.Count)];
        }
    }
}
