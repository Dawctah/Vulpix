using Vulpix.Domain.Models;

namespace Vulpix.Domain.Repositories
{
    public interface IHobbyFile
    {
        IEnumerable<Item> GetAllItems(HobbyType hobbyType);

        string LastSelectedHobbyTypeString { get; set; }

        void Update(Item item, bool organize = true);
        void Insert(Item item);
        void Delete(Item item);
    }
}