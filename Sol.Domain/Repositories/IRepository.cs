using Sol.Domain.Models;

namespace Sol.Domain.Repositories
{
    public interface IRepository<T>
        where T : AggregateRoot
    {
        void Insert(T item);
        void InsertMany(IEnumerable<T> items);
        void Update(T item);
        void Remove(T item);
        IEnumerable<T> GetAll();
    }
}