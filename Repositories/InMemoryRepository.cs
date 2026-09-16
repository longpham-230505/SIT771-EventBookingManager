using System.Collections.Generic;
using System.Linq;
using EventVenueBookingManager.Common;
using EventVenueBookingManager.Interfaces;

namespace EventVenueBookingManager.Repositories
{
    // Repository pattern: simple in-memory storage behind IRepository<T>.
    public class InMemoryRepository<T> : IRepository<T> where T : IEntity
    {
        private readonly List<T> _items = new();

        public void Add(T item) => _items.Add(item);

        public void Remove(string id) => _items.RemoveAll(i => i.Id == id);

        public T? FindById(string id) => _items.FirstOrDefault(i => i.Id == id);

        public IEnumerable<T> GetAll() => _items.AsReadOnly();
    }
}
