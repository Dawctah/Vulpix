using Sol.Domain.Models;

namespace Sol.Domain.Queries
{
    public static class GetAllHobbiesQuery
    {
        public static IEnumerable<Hobby> GetHobbies()
        {
            var result = new List<Hobby>();
            var hobbyTypes = typeof(Hobby).Assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(Hobby)) && !type.IsAbstract);
            foreach (var type in hobbyTypes)
            {
                result.Add((Activator.CreateInstance(type) as Hobby)!);
            }

            return result.OrderBy(hobby => (int)hobby.Type);
        }
    }
}
