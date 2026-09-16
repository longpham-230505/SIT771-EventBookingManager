
using System.Collections.Generic;
using EventVenueBookingManager.Common;

/**
For this project, I won't connect the program to a database server
but instead, I will mimic a simple database system with repository
pattern
*/

namespace EventVenueBookingManager.Interfaces
{
    public interface IRepository<T> where T : IEntity
    {
        void Add(T item);
        void Remove(string id);
        T? FindById(string id);
        IEnumerable<T> GetAll();
    }
}
