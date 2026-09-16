using System.Collections.Generic;
using EventVenueBookingManager.Models;

namespace EventVenueBookingManager.Interfaces
{
    // Strategy pattern conflict checking
    public interface IConflictStrategy
    {
        bool HasConflict(Booking candidate, IEnumerable<Booking> existingBookings);
    }
}
